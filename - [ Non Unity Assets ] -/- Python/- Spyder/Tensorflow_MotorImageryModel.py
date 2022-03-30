import tensorflow
from tensorflow import keras
from tensorflow.python.keras.callbacks import TensorBoard
from time import time
import numpy
import csv
from sklearn.preprocessing import LabelEncoder

path = "D:\My Data\EEG Data\S001R14"





#%%################################
#   - Read in Training Data -     #
###################################
print("\n - Reading in training data. \n")

dataFile = open(path + ".csv", "r", newline = '')
markerFile = open(path + "_Markers.csv", "r", newline = '')

dataReader = csv.reader(dataFile, delimiter=",")
markerReader = csv.reader(markerFile, delimiter=",")

dataList = list(dataReader)
markerList = list(markerReader)

dataList.pop(0)
data_NumPyArray = numpy.array(dataList).astype(float)

markerList.pop(0)
marker_NumPyArray = numpy.array(markerList).astype(str)

dataFile.close()
markerFile.close()





#%%###################################
#   - Encode Markers into ID's -     #
######################################
print("\n - Encode the markers into unique ID's. \n")

encoder = LabelEncoder()

markerIDs = encoder.fit_transform(marker_NumPyArray)
print(markerIDs)





#%%###########################
#   - Generate Network -     #
##############################
print("\n - Generating Network. \n")

model = keras.Sequential([
    keras.layers.Flatten(input_shape=(64,)),
    keras.layers.Dense(units=200, activation=("relu")),
    keras.layers.Dense(units=20, activation=("relu")),
    keras.layers.Dense(units=3, activation=("sigmoid"))
    ])


tensorboard = TensorBoard(log_dir=("logs/{}".format(time())))

model.compile(loss = keras.losses.SparseCategoricalCrossentropy(), optimizer = tensorflow.optimizers.Adam(), metrics=['accuracy'])

model.fit(data_NumPyArray, markerIDs, epochs=100, callbacks=([tensorboard]))
model.summary()

print(model.weights)





#%%#####################
#   - Prediction -     #
########################
print("\n - Predicting the network output. \n")

print("Proberbility Prediction")
print(model.predict_proba(data_NumPyArray))  

print("Evaluated Score")
evaluationScore = model.evaluate(data_NumPyArray, markerIDs, verbose=1);
print("Loss: " + str(evaluationScore[0]) + ", Accuracy: " + str(evaluationScore[1]))





#%%#############################
#   - Save The Model -     #
################################
print("\n - Saving the tensorflow model. \n")

modelName = "MotorImageryModel"

model.save('D:\My Data\Tensorflow Models\\' + modelName + '.h5')
print("Data saved to 'D:\My Data\Tensorflow Models\\" + modelName + ".h5'")