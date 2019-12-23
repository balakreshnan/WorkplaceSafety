# Inferencing Model - Jetson Nano Without Docker container

## Inferencing the workplace safety model using Nvidia Jetson Nano without docker container

When exported the model from custom vision there are options to build models using tensorflow and embedd that into docker for various operating systems like Rasperry Pi, linux and windows and Tensorflow. To do this we need to re train the model using option to download for tensorflow or other mobile options.

## Get started with NVidia Jetson Nano

Get the NVidia Jetson Nano up and running. Install Tensorflow as per Jetson nano Zoo link documentation available in Jetson Nano desktop screen.

## Install Pillow

Pillow package was not found. So i had to install in NVidia jetson Nano.
```
python3 -m pip install Pillow
```

## where is the model available

Go to link: https://github.com/balakreshnan/WorkplaceSafety/tree/master/jetsonnano
The model pb file, labels files and inferencing code are also available. simple_camera.py has the code for loading tensorflow model and use the gpu to inference.

## Code below.

The below code was readily available sample code modified to inference custom vision tensorflow model.

```
# MIT License
# Copyright (c) 2019 JetsonHacks
# See license
# Using a CSI camera (such as the Raspberry Pi Version 2) connected to a
# NVIDIA Jetson Nano Developer Kit using OpenCV
# Drivers for the camera and OpenCV are included in the base image

import cv2

# gstreamer_pipeline returns a GStreamer pipeline for capturing from the CSI camera
# Defaults to 1280x720 @ 60fps
# Flip the image by setting the flip_method (most common values: 0 and 2)
# display_width and display_height determine the size of the window on the screen

import sys
import tensorflow as tf
import numpy as np
from PIL import Image
from object_detection import ObjectDetection

MODEL_FILENAME = 'model.pb'
LABELS_FILENAME = 'labels.txt'

class TFObjectDetection(ObjectDetection):
    """Object Detection class for TensorFlow"""

    def __init__(self, graph_def, labels):
        super(TFObjectDetection, self).__init__(labels)
        self.graph = tf.compat.v1.Graph()
        with self.graph.as_default():
            input_data = tf.compat.v1.placeholder(tf.float32, [1, None, None, 3], name='Placeholder')
            tf.import_graph_def(graph_def, input_map={"Placeholder:0": input_data}, name="")

    def predict(self, preprocessed_image):
        inputs = np.array(preprocessed_image, dtype=np.float)[:, :, (2, 1, 0)]  # RGB -> BGR

        with tf.compat.v1.Session(graph=self.graph) as sess:
            output_tensor = sess.graph.get_tensor_by_name('model_outputs:0')
            outputs = sess.run(output_tensor, {'Placeholder:0': inputs[np.newaxis, ...]})
            return outputs[0]


def gstreamer_pipeline(
    capture_width=1280,
    capture_height=720,
    display_width=1280,
    display_height=720,
    framerate=60,
    flip_method=0,
):
    return (
        "nvarguscamerasrc ! "
        "video/x-raw(memory:NVMM), "
        "width=(int)%d, height=(int)%d, "
        "format=(string)NV12, framerate=(fraction)%d/1 ! "
        "nvvidconv flip-method=%d ! "
        "video/x-raw, width=(int)%d, height=(int)%d, format=(string)BGRx ! "
        "videoconvert ! "
        "video/x-raw, format=(string)BGR ! appsink"
        % (
            capture_width,
            capture_height,
            framerate,
            flip_method,
            display_width,
            display_height,
        )
    )


def show_camera():
    # To flip the image, modify the flip_method parameter (0 and 2 are the most common)
    print(gstreamer_pipeline(flip_method=0))
    cap = cv2.VideoCapture(gstreamer_pipeline(flip_method=0), cv2.CAP_GSTREAMER)

    # Load a TensorFlow model
    graph_def = tf.compat.v1.GraphDef()
    with tf.io.gfile.GFile(MODEL_FILENAME, 'rb') as f:
        graph_def.ParseFromString(f.read())

    # Load labels
    with open(LABELS_FILENAME, 'r') as f:
        labels = [l.strip() for l in f.readlines()]

    od_model = TFObjectDetection(graph_def, labels)

    if cap.isOpened():
        window_handle = cv2.namedWindow("CSI Camera", cv2.WINDOW_AUTOSIZE)
        # Window
        while cv2.getWindowProperty("CSI Camera", 0) >= 0:
            ret_val, img = cap.read()
            cv2.imshow("CSI Camera", img)
            cv2.imwrite("image.jpg", img)

            image = Image.open("image.jpg")
            predictions = od_model.predict_image(image)
            print(predictions)
            # This also acts as
            keyCode = cv2.waitKey(30) & 0xFF
            # Stop the program on the ESC key
            if keyCode == 27:
                break
        cap.release()
        cv2.destroyAllWindows()
    else:
        print("Unable to open camera")


if __name__ == "__main__":
    show_camera()
```

Save the file and run and output show the object detected and also it's probability.

```
python3 simple_camera.py
```

Window will popup for camera and please click close to see the output. Future version will update more clear code.

## Outcome

Was able to download the custom vision model export as tensorflow and running inside NVidia jetson nano without doing any modification.
When i ran the rasperry pi model it took about close to 4 seconds to inference. When i ran with NVidia jetson nano was less than a second 
using jetson's GPU.
