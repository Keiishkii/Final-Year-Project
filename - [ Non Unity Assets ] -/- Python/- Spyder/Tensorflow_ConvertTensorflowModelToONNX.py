from tensorflow.python.keras.models import load_model
import onnx
import keras2onnx

#%%######################
#   - Clear Console -   #
#########################
# Clears the console on starting the python program.
print("\033[H\033[J") 





#%%#########################
#   - Global Variables -   #
############################
# Global variable storing the path of the model and its name.
# This is both the input and output path of the model.
modelSavingDataPath = "D:\My Data\Tensorflow Models\\"
modelName = "Focus Model_4"





#%%#########################
#   - FUNCTION DEFINES -   #
############################
#%%######################
#   - Convert Model -   #
#########################
# Using the Keras2Onnx Library, converts the .h5 model generated in Keras to a .onnx file.
# The .onnx file is a portable version of ANN's
def convert_model():
    model = load_model(modelSavingDataPath + modelName + '.h5')
    onnx_model = keras2onnx.convert_keras(model, model.name)
    
    onnx.save_model(onnx_model, modelSavingDataPath + modelName + '.onnx')
    print("Converted file")





#%%################
#   - Program -   #
###################
# Calls the function to convert the models
convert_model()