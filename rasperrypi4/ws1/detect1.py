import json
import os
import io
import time

from picamera import PiCamera
from time import sleep
# Imports for the REST API
from flask import Flask, request, jsonify

# Imports for image procesing
from PIL import Image

# Imports for prediction
from predict import initialize, predict_image, predict_url

if __name__ == '__main__':
    # Load and intialize the model
    initialize()
    
    
 
    camera = PiCamera()

    try:
        while True:
            n = input("\n Type something to quit: ")
            
            if n == "q":
                break
            
            start = time.time()
            camera.start_preview(alpha=200)
            camera.resolution = (1980, 1080)
            camera.framerate = 15
            #sleep(5)
            #sleep(1)
            camera.capture('image.jpg')

            camera.stop_preview()
            
            image = Image.open("image.jpg")
            
            result = predict_image(image)
             
            #print("Results from Custom Vision Model: ")
            #print(result)
            
            #print("Results from Custom Vision Model: Predictions ")
            #print(result['predictions'])
            print()
            
            i = 0
            for tag in result['predictions']:
                print("ID: " + str(tag['tagId']) + " Object detected : " + tag['tagName'] + " Confidence: " + '{0:f}'.format(tag['probability']))
                print()

            elapsed_time_fl = (time.time() - start)
            print(elapsed_time_fl)
        
            
        
    finally:
        camera.close()
    
