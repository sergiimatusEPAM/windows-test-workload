echo "SNAPSHOT_VERSION: ${BUILD_NUMBER}"
////////////////////////
// put variables here:
def appName = "windows-test-workload"
def appJSONPath = "${appName}/Marathon-templates/${appName}.json"
def gitURL = "git clone git://github.com/dcos/${appName}.git"

def DockerRegToken = "DockerHub_token"
def DockerfilePath = "https://raw.githubusercontent.com/dcos/windows-test-workload/master/Dockerfile"
def DockerAppImageName = "dcos/windows-test-workload"

def NexusRepoToken = "Nexus_token"
def NexusPort = "27092"
def NexusRepoName = "dotnet-sample"

def dynamicJenkinsSlave = "mesos-windows"
def staticJenkinsSlave = "build-docker"

def buildNumber = "${BUILD_NUMBER}"
//
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
    stage("Upload Snapshot to Nexus"){
        withCredentials([[$class: 'UsernamePasswordMultiBinding', credentialsId: "${NexusRepoToken}", usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD']]) {
            bat "curl -v -u %USERNAME%:%PASSWORD% --upload-file package.${buildNumber}.zip http://nexus.marathon.mesos:${NexusPort}/repository/${NexusRepoName}/0.1-SNAPSHOT/${appName}.zip"
        }
    }
    stage("Upload Release to Nexus "){
        withCredentials([[$class: 'UsernamePasswordMultiBinding', credentialsId: "${NexusRepoToken}", usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD']]) {
            bat "curl -v -u %USERNAME%:%PASSWORD% --upload-file package.${buildNumber}.zip http://nexus.marathon.mesos:${NexusPort}/repository/${NexusRepoName}/RELEASE/${appName}-0.1.${buildNumber}.zip"
        }
    }
}
node("${staticJenkinsSlave}"){
    stage("Build Docker image"){
        bat """
            docker build -t ${DockerAppImageName} ${DockerfilePath} --no-cache --build-arg URL_TO_APP_SNAPSHOT=http://nexus.marathon.mesos:${NexusPort}/repository/${NexusRepoName}/0.1-SNAPSHOT/${appName}.zip
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
}
node("${dynamicJenkinsSlave}") {
    stage("Publish service") {
        marathon(
            url: "http://marathon.mesos:8080",
            forceUpdate: true,
            docker: "${DockerAppImageName}:0.${buildNumber}",
            filename: "${appJSONPath}")
    }
}