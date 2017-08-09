#tool nuget:?package=NUnit.ConsoleRunner&version=3.6.0
#tool "nuget:?package=ReportUnit"
#tool "docfx.console"


#addin "Cake.DocFx"
#addin "SharpZipLib"
#addin "Cake.FileHelpers"
#addin "Cake.Compression"
#addin "MagicChunks"

//#addin "nuget:?package=Cake.StyleCop&version=1.1.3"
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release"); // by default will compile release 

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutions = GetFiles("./**/*.sln");
var solutionFile = solutions.First();

var solutionPaths = solutions.Select(solution => solution.GetDirectory());
var testResultFolder = MakeAbsolute(Directory(Argument("testResultsPath", "./test-results")));
var buildResultFolder = MakeAbsolute(Directory(Argument("buildResultsPath","./build-results")));

var testResultsFilePath = testResultFolder + "/TestResult.xml";
var testOutputFile = File(testResultsFilePath);

var publishDirectory = MakeAbsolute(Directory(Argument("publishFolder", "./publish")));

var buildResultsFilePath = buildResultFolder + "/build.log";
var buildResultsOutputFile = File(buildResultsFilePath);

var publishProjectDirectory = MakeAbsolute(Directory("./src/App"));
var publishProjectPath = publishProjectDirectory + "/App.sln";
var publishProjectFile = File(publishProjectPath);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    // Executed BEFORE the first task.
	
	if (!DirectoryExists(testResultFolder))
    {
        CreateDirectory(testResultFolder);
    }
	
	
	if (!DirectoryExists(publishDirectory))
    {
        CreateDirectory(publishDirectory);
    }
	
	if (!DirectoryExists(buildResultFolder))
    {
        CreateDirectory(buildResultFolder);
    }
	
    Information("Running tasks...");
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Description("Cleans all directories that are used during the build process.")
    .Does(() =>
{
	CleanDirectory(testResultFolder);
	CleanDirectory(publishDirectory);
    // Clean solution directories.
    foreach(var path in solutionPaths)
    {
        Information("Cleaning {0}", path);
        CleanDirectories(path + "/**/bin/" + configuration);
        CleanDirectories(path + "/**/obj/" + configuration);
    }
});



Task("Restore")
    .Description("Restores all the NuGet packages that are used by the specified solution.")
    .Does(() =>
{
    // Restore all NuGet packages.
    foreach(var solution in solutions)
    {
        Information("Restoring {0}...", solution);
        NuGetRestore(solution);
    }
});

//Task("StyleCop")
//	.Does(() => 
//	{
//        StyleCopAnalyse(settings => settings
//			.WithSolution(solutionFile)
//			.WithSettings(File("./Settings.StyleCop"))
//			.ToResultFile(buildResultFolder + File("StyleCopViolations.xml")));
//	});

// Build the DocFX documentation site
Task("Documentation")
    .Does(() =>
{
    DocFxMetadata("./doc/docfx.json");
    DocFxBuild("./doc/docfx.json");

    CreateDirectory("artifacts");
    // Archive the generated site
    ZipCompress("./doc/_site", "./artifacts/site.zip");
});

Task("Build")
    .Description("Builds all the different parts of the project.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
	//.IsDependentOn("StyleCop")
    .Does(() =>
{
    // Build all solutions.
    foreach(var solution in solutions)
    {
        Information("Building {0}", solution);
        MSBuild(solution, settings =>
            settings.SetPlatformTarget(PlatformTarget.MSIL)
                .WithProperty("TreatWarningsAsErrors","true")
                .WithTarget("Build")
                .SetConfiguration(configuration)
				.AddFileLogger(
        new MSBuildFileLogger {
            Verbosity = Verbosity.Normal,
            LogFile = buildResultsOutputFile
          }));
    }
	 
	CleanDirectories("./**/obj");
});

Task("Publish")
	.IsDependentOn("Build")
	.Does(() => 
{
	EnsureDirectoryExists(publishDirectory);
	// copy all things into the publish folder 
	
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
	EnsureDirectoryExists(testResultFolder);
	
    NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        Results = testOutputFile,
		Verbose = true,
		Workers = 30
        });
});

Task("Generate-Unit-Test-Report")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() => 
	{
		ReportUnit(testResultFolder, testResultFolder, new ReportUnitSettings());
	});

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .Description("This is the default task which will be ran if no specific target is passed in.")
    .IsDependentOn("Generate-Unit-Test-Report");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);