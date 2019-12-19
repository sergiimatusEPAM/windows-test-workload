echo "SNAPSHOT_VERSION: ${BUILD_NUMBER}"
////////////////////////
// put variables here:
def appName = "windows-test-workload"
def appJSONPath = "${appName}/Marathon-templates/${appName}.json"
def gitURL = "git://github.com/sergiimatusepam/${appName}.git"

def DockerRegToken = "DockerHub_token"
def DockerfilePath = "https://raw.githubusercontent.com/sergiimatusepam/windows-test-workload/master/Dockerfile"
def DockerAppImageName = "sergiimatusepam/windows-test-workload"


def dynamicJenkinsSlave = "windows"

def buildNumber = "${BUILD_NUMBER}"

def jobName = "${JOB_NAME}"
////////////////////////

node("${dynamicJenkinsSlave}") {
    stage("Checkout of git repo (cmd)") {
        bat "IF not exist ${appName} (git clone ${gitURL}) else (cd ${appName} && git pull)";
    }
    stage("Clean") {
        bat "cd ${appName} && dotnet clean"
    }
    stage("Build") {
        bat "cd ${appName} && dotnet build"
    }
    stage("Publish, pack into zip") {
        bat "cd ${appName} && dotnet publish -o .\\..\\..\\target"
        bat "7z.exe a -mmt2 -tzip package.${buildNumber}.zip target"
    }
}
