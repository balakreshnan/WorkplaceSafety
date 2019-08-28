# Camera tagging module deployment in Iot Hub Edge using Azure Portal
Deploy Camera tagging module into Vision AI kit using Azure protal

## Deployment

### Deploy Camera Tagging Module

Go to the Iot Hub where Vision Kit is configured.

> Click on Iot Edge select the vision kit device. Once selected you should see the list of all modules.

> Select Add Modules and then click New Iot Edge Module.

Note: i am using existing docker image that was created.
- Name the module as CameraTaggingModule
- specify the docker image URI as : ebertrams/camerataggingmodule:0.0.62-arm32v7
- Here is the container options
```
{
  "ExposedPorts": {
    "3000/tcp": {},
    "3002/tcp": {},
    "3003/tcp": {}
  },
  "HostConfig": {
    "PortBindings": {
      "3000/tcp": [
        {
          "HostPort": "3000"
        }
      ],
      "3002/tcp": [
        {
          "HostPort": "3002"
        }
      ],
      "3003/tcp": [
        {
          "HostPort": "3003"
        }
      ]
    }
  }
}
```
- Environment Variables
RTSP_IP
RTSP_PORT
RTSP_PATH
REACT_APP_LOCAL_STORAGE_MODULE
REACT_APP_LOCAL_STORAGE_PORT

Click Save and then click Next for the routes and click finish.

Wait for few minutes to get the image deployed.

### Deploy Blob Storage container

> Now lets deploy the blob storage continer for storing images

> Go back to Add modules and click Iot Edge new module 

- Module name azureblobstorageoniotedge
- Docker image URI: mcr.microsoft.com/azure-blob-storage:latest
- Continer options

```

{
  "Env": [
    "LOCAL_STORAGE_ACCOUNT_NAME=<blob account name>",
    "LOCAL_STORAGE_ACCOUNT_KEY=<key>"
  ],
  "HostConfig": {
    "Binds": [
      "/data/containerdata:/<containername>"
    ],
    "PortBindings": {
      "11002/tcp": [
        {
          "HostPort": "11002"
        }
      ]
    }
  }
}
```

- Env Variable  - nothing here

Click Next and go to routes

- Route should be

```
{
  "routes": {
    "VisionModule": "FROM /messages/* INTO $upstream",
    "azureblobstorageoniotedgeToIoTHub": "FROM /messages/modules/azureblobstorageoniotedge/outputs/* INTO $upstream"
  }
}
```

- the first route to send the Vision Sample Module to send detected obejct to iot hub
- the second is for saving the file for blob storage.

