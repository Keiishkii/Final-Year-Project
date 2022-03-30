import tensorflow
from tensorflow import keras
from tensorflow.python.keras.callbacks import TensorBoard
from time import time
import numpy

#%%

inputValues = numpy.array([[0,0],[0,1],[1,0],[1,1]])
inputResults = numpy.array([[0],[1],[1],[0]])

print(inputValues)
print(inputResults)

#%%

model = keras.Sequential([
    keras.layers.Flatten(input_shape=(2,)),
    keras.layers.Dense(units=50, activation=("sigmoid")),
    keras.layers.Dense(units=2, activation=("sigmoid"))
    ])


tensorboard = TensorBoard(log_dir=("logs/{}".format(time())))

model.compile(loss = keras.losses.SparseCategoricalCrossentropy(), optimizer = tensorflow.optimizers.Adam(), metrics=['accuracy'])

model.fit(inputValues, inputResults, epochs=25000, callbacks=([tensorboard]))
model.summary()

print(model.weights)
#%%

print(model.predict_proba(inputValues))

#%%

model.save('D:\My Data\Tensorflow Models\XOR.h5')
print("Data saved to 'D:\My Data\Tensorflow Models\XOR.h5'")