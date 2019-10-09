
# Windows Test Workload

Show case of building a .NET Core application which is using DC/OS, Marathon and Jenkins to run workloads on private DC/OS Windows agents.

## Current actions in the workflow are performed:
- spin up dynamic Jenkins slave on Windows in Docker container,
- checkout of GitHub repository,
- run “dotnet build”,
- pack .NET Core application into zip,
- upload to Nexus artifacts repository,
- build the application in a Docker image
- and finally publishes the image to DockerHub.

## Usage example:
### at Windows slave nodes:
- install install Java with chocolatey (`choco install jdk8 -y`)

### at DC/OS cluster setup following services:
- Jenkins from [Marathon-templates/jenkins.json](https://github.com/dcos/windows-test-workload/blob/master/Marathon-templates/jenkins.json)
- Nexus from DC/OS catalog, just specify the static port, for instance, `27092` as it has been used in the pipeline. Be aware the ports need to be opened on firewall side
- Marathon-lb from DC/OS catalog

### at DockerHub:
- sign up
- create `windows-test-workload` repository. Grab full name, i.e. `mesosphere/windows-test-workload`

### at Nexus:
- login with default credentials
- create raw(hosted) repository called `dotnet-sample` and uncheck box "Validate format"

### at Jenkins:
- Submit (Global Credentials -> 'Username and password')[https://jenkins.io/doc/book/using/using-credentials/#adding-new-global-credentials] for Nexus and DockerHub. Name them `Nexus_token` and `DockerHub_token` respectively

- Install [Credentials Binding](https://plugins.jenkins.io/credentials-binding) plugin

- Create a Scripted Pipeline Job with defined at `Jenkinsfile`.

- Install [Credentials Binding](https://plugins.jenkins.io/credentials-binding) plugin

- Place name of Docker Image which is going to be used to build dotnet application at "Jenkins on Mesos" plugin. I.e. `mesosphere/dotnet-builder`

  Go to Manage Jenkins -> Configure System -> follow "Advanced" after "Mesos Cloud" section -> follow "Advanced" after "Use Docker Containerizer" -> Use `mesosphere/dotnet-builder:latest`
  Also you may use your own image, see [dotnet-builder/DockerFile.jenkins.windows.slave](https://github.com/dcos/windows-test-workload/blob/master/dotnet-builder/DockerFile.jenkins.windows.slave) file for pre-configuration, depending on your needs.

- Add a static node named `build-docker` as a Jenkins slave on Windows for building a docker images. Note, the step requires as there is currently limitation - Docker in Docker concept doesn't work on Windows yet.
  
  For this follow [Step by step guide to set up master and agent machines on Windows](https://wiki.jenkins.io/display/JENKINS/Step+by+step+guide+to+set+up+master+and+agent+machines+on+Windows)  
  Please note that "Launch agent via Java Web Start" method was renamed to "Launch agent by connecting it to master"

- Submit (Global Credentials -> 'Username and password')[https://jenkins.io/doc/book/using/using-credentials/#adding-new-global-credentials] for Nexus and DockerHub. Name them `Nexus_token` and `DockerHub_token` respectively

- Update [Jenkinsfile](https://github.com/dcos/windows-test-workload/blob/master/Jenkinsfile) file with setup details, if those differs from default

- Create a Scripted Pipeline Job with defined at `Jenkinsfile`. 
