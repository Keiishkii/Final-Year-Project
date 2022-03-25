from tensorflow.python.keras import backend as K
from tensorflow.python.keras.models import load_model
import onnx
import keras2onnx

model = load_model('D:\My Data\Tensorflow Models\XOR.h5')
onnx_model = keras2onnx.convert_keras(model, model.name)


onnx.save_model(onnx_model, "D:\My Data\Tensorflow Models\BinaryXOR.onnx")
print("Converted file")