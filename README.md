
# Windows Test Workload

Show case of building a .NET Core application which is using DC/OS, Marathon and Jenkins to run workloads on private DC/OS Windows agents.

## Current actions in the workflow are performed:
- spin up dynamic Jenkins slave on Windows in Docker container,
- checkout of GitHub repository,
- run “dotnet build”,
- pack .NET Core application into zip,
- build the zipped application in a Docker image
- publishes the image to DockerHub
- spin up the application using Marathon plugin from pre-setup [Marathon-templates/windows-test-workload.json](https://github.com/dcos/windows-test-workload/blob/master/Marathon-templates/windows-test-workload.json)

## Usage example:

### at DC/OS cluster setup following services:
- Jenkins Master from [Marathon-templates/jenkins.json](https://github.com/dcos/windows-test-workload/blob/master/Marathon-templates/jenkins.json)
- Marathon-lb from DC/OS catalog

### at DockerHub:
- sign up
- create `windows-test-workload` repository. Grab full name, i.e. `mesosphere/windows-test-workload`. Configuration of DockerHub is specified in [Jenkinsfile](https://github.com/dcos/windows-test-workload/blob/master/Jenkinsfile)

### at Jenkins:
- Install [Credentials Binding](https://plugins.jenkins.io/credentials-binding) plugin

- Submit (Global Credentials -> 'Username and password')[https://jenkins.io/doc/book/using/using-credentials/#adding-new-global-credentials] for DockerHub. Name such `DockerHub_token`

- Update [Jenkinsfile](https://github.com/dcos/windows-test-workload/blob/master/Jenkinsfile) file with setup details, if those differs from default

- Create a Scripted Pipeline Job with defined at `Jenkinsfile`.

- Place name of Docker Image which is going to be used to build dotnet application at "Jenkins on Mesos" plugin under `windows` label configuration. I.e. `mesosphere/dotnet-builder`

  Go to Manage Jenkins -> Configure System -> follow "Advanced" after "Mesos Cloud" section -> follow "Advanced" after "Use Docker Containerizer" -> Use `mesosphere/dotnet-builder:latest`. Under Mesos Cloud -> Label:Windows -> "Volumes" section -> press "Add Volume" , put Container and Host Path `\\.\pipe\docker_engine`. It requires to support backing docker image in the containerized slave (Docker in Docker on Windows).
  Also you may use your own image, see [dotnet-builder/DockerFile.jenkins.windows.slave](https://github.com/dcos/windows-test-workload/blob/master/dotnet-builder/DockerFile.jenkins.windows.slave) file for pre-configuration, depending on your needs.