from tensorflow.python.keras.models import load_model
import onnx
import keras2onnx

#%%########################
#   - Clear Console -     #
###########################
print("\033[H\033[J") 





#%%###########################
#   - Global Variables -     #
##############################
modelSavingDataPath = "D:\My Data\Tensorflow Models\\"
modelName = "Focus Model_4"





#%%###########################
#   - Global Variables -     #
##############################
model = load_model(modelSavingDataPath + modelName + '.h5')
onnx_model = keras2onnx.convert_keras(model, model.name)

onnx.save_model(onnx_model, modelSavingDataPath + modelName + '.onnx')
print("Converted file")