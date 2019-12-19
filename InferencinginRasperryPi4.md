# Inferencing Model - Raspberry PI 4 Without Docker container

## Inferencing the workplace safety model using raspberry pi 4 without docker container

When exported the model from custom vision there are options to build models using tensorflow and embedd that into docker for various operating systems like Raspberry PI, linux and windows. To do this we need to re train the model using option to download for tensorflow or other mobile options.

To get started with Raspberry PI 4 first get the raspberry pi 4 to working condition by installing the operating system. Next is to update python3 and do that please follow the link below
https://github.com/instabot-py/instabot.py/wiki/Installing-Python-3.7-on-Raspberry-Pi

Once python3 is installed please follow the instruction to install tensorflow. Before installing tensorflow please update the operating system.
```
sudo apt-get update
sudo apt-get upgrade
```

## Tensorflow installation
open a terminal in raspberry pi 4 and type the below commands
```
sudo apt-get install libatlas-base-dev
sudo pip3 install tensorflow
```

To test tensoflow is installed.Go to Programming in the main menu of the Raspberry pi desktop and navigate to Thonny python ide and select that. Click New and type the below:
import tensorflow as tf
print(tf.__version__)

Now click Run and version of tensorflow should be displayed in the bottom Shell output window.

For more details please use this link:
https://magpi.raspberrypi.org/articles/tensorflow-ai-raspberry-pi

Now the Raspberry Pi is ready to write the inferencing code.

Go to link: https://github.com/balakreshnan/WorkplaceSafety/tree/master/TensorFlowModelsforotherplatform
DOwnload the model for ARM.
https://github.com/balakreshnan/WorkplaceSafety/blob/master/TensorFlowModelsforotherplatform/34f05b15e0be4bcfabbcc0a216183ec8.DockerFile.ARM.zip

Download the files into /home/pi/examples folder. unzip the file inside that folder.
Now create a new folder called ws (workplacesafety). now copy all the content unzipped into ws folder.
Download few vest images to test the inferencing model. i downloaded it from google.com

now it is time to create the inferencing file. create a new python file called detect.py

```
import json
import os
import io

# Imports for the REST API
from flask import flask, request, jsonify

# Imports for image processing
from PIL import Image

# imports for prediction
from predict import initialize, predict_image, predict_url

if __name__ == '__main__'
    # load and initialize the mode
    initialize()

    image = Image.open("vest1.jpg") # change the file name to what you downloaded.
    result = predict_image(image)

    Print()
    i = 0
    for tag in result['predictions']:
        print("ID: " + str(tag['tagId']) + " Object Detected: " + tag['tagName'] + " Confidence: " + '{0:f}.format(tag['probability']))
        print()

```

Save the file and run and output show the object detected and also it's probability.

