# Overview
Demo .NET Core 2.1 generic host background application running in OpenShift. Application pulls messages from Azure Service Bus and writes them to the console log.

## Concepts

1. .NET Core
	1. Generic Host
	2. Configuration
	3. Hosted Services
2. OpenShift
	1. [Config Map](deploy/configmap.yml)
	2. [Secrets](deploy/secrets.yml)
	3. [Deployment Config](deploy/kubedeploy.yml)
	4. Namespaces/Projects
	5. OC CLI

## Environment Setup

* install Docker for Windows (use Linux containers)
* install Visual Studio Code
* `oc cluster up`
* create Azure Service Bus Queue
	* Configure `MessageProcessor.ConnectionString` in [appsettings.Development.json](/SampleHosting/appsettings.Development.json)
	* Configure `MessageProcessor.QueueName` in [appsettings.json](/SampleHosting/appsettings.Development.json)
* (optional) install the dotnet-azsb (Azure Service Bus CLI). Provides the ability to create SaS tokens used to post http messages to the queue.
	* dotnet azsb queue sas create -n <policyname> -k <key> <servicebus queue url>/messages

## OpenShift Integration

### Setup

* Get registry url: 
	* login to OpenShift CLI as admin: `oc login -u System:admin`
	* `oc get svc -n default | grep registry`
* Get OpenShift access token:
	* login to OpenShift CLI as developer: `oc login -u developer -p test`  
	* `oc whoami -t`
* Login to OpenShift registry: `docker login -u developer -p k5iGmgVEj_O3Rx9JYOLC2ivoFoK1uTtkC-rqg25RSWw 172.30.1.1:5000`
* Create project:
	* `oc new-project myproject`

### CI/CD

* Build image: `docker-compose build`
* Tag image for OpenShift: `docker tag samplehosting:latest 172.30.1.1:5000/myproject/samplehosting:1.0.0`
* Push to registry: `docker push 172.30.1.1:5000/myproject/samplehosting:1.0.0`
* Apply config map: `oc apply -f secrets.yml`
* Apply secrets: `oc apply -f configmap.yml`
* Apply app
	* via UI
		* create OpenShift app from image: `oc new-app myproject/samplehosting --name=mysamplehosting`
	* via yaml
		* `oc apply -f kubedeploy.yml -n myproject`

### Troubleshooting

* Execute commands: `oc exec -it mysamplehosting-1-6mtlq /bin/bash`
* Remote Shell: `oc rsh mysamplehosting-1-6mtlq`
* Logs: `oc logs mysamplehosting-1-6mtlq`