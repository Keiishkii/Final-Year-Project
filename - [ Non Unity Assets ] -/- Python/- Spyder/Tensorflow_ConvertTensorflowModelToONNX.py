from tensorflow.python.keras.models import load_model
import onnx
import keras2onnx

modelName = "MotorImageryModel"





model = load_model('D:\My Data\Tensorflow Models\\' + modelName + '.h5')
onnx_model = keras2onnx.convert_keras(model, model.name)

onnx.save_model(onnx_model, 'D:\My Data\Tensorflow Models\\' + modelName + '.onnx')
print("Converted file")