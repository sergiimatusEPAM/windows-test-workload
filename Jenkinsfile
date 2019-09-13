echo "SNAPSHOT_VERSION: ${BUILD_NUMBER}"
////////////////////////
// put variables here:
def appName = "windows-test-workload"
def gitHubAccount = "dcos"
def dockerHubAccount = "mesosphere"
def dynamicJenkinsSlave = "windows"
//
def appJSONPath = "${appName}/Marathon-templates/${appName}.json"
def gitURL = "git://github.com/${gitHubAccount}/${appName}.git"

def DockerRegToken = "DockerHub_token"
def DockerfilePath = "Dockerfile"
def DockerAppImageName = "${dockerHubAccount}/${appName}"

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
		bat "copy /b/v/y package.${buildNumber}.zip demoapp.zip"
    }
    stage("Build Docker image"){
        bat """
            docker build -t ${DockerAppImageName} ${DockerfilePath} --no-cache
            docker tag ${DockerAppImageName}:latest ${DockerAppImageName}:0.${buildNumber}
        """
    }
    stage("Publish to DockerHub"){
        withCredentials([[$class: 'UsernamePasswordMultiBinding', credentialsId: "${DockerRegToken}", usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD']]) {
            bat """
                echo | set /p="%PASSWORD%" | docker login -u %USERNAME% --password-stdin
                docker push ${DockerAppImageName}:0.${buildNumber}
                docker push ${DockerAppImageName}:latest
            """
        }
    }
    stage("Publish service") {
        marathon(
            url: "http://marathon.mesos:8080",
            forceUpdate: true,
            docker: "${DockerAppImageName}:0.${buildNumber}",
            filename: "${appJSONPath}")
    }
}
