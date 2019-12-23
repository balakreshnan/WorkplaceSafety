import json
import os
import io

# Imports for the REST API
from flask import Flask, request, jsonify

# Imports for image procesing
from PIL import Image

# Imports for prediction
from predict import initialize, predict_image, predict_url

if __name__ == '__main__':
    # Load and intialize the model
    initialize()
    
    image = Image.open("vest1.jpg")
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

    
    
    
    