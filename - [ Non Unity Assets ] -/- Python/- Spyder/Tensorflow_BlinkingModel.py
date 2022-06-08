#%%
import keras_tuner
import numpy
import csv
import tensorflow
from tensorflow import keras
from tensorflow.keras import layers
from sklearn.preprocessing import LabelEncoder
from tqdm import tqdm

#%%######################
#   - Clear Console -   #
#########################
# Clears the console on starting the python program.
print("\033[H\033[J") 





#%%#########################
#   - Global Variables -   #
############################
# Global variables used for generating and training the blink classificaiton neural network.
# Path of the input data contianing blinks, used to train the network.
_blinkingTrainingDataPath = "D:\\My Data\\[ EMG Data ]\\[ CSV Files ]\\[ Facial ]\\Blinking\\"
_blinkingTrainingDataFileNames = []
_validationBlinkingTrainingDataFileNames = [
    "96",
    "97",
    "98",
    "99",
    "100"
    ]
 
# Path of the input data contianing normal eye movement, used to train the network.
_normalEyeMovementTrainingDataPath = "D:\\My Data\\[ EMG Data ]\\[ CSV Files ]\\[ Facial ]\\Normal Eye Movement\\"
_normalEyeMovementTrainingDataFileNames = []
_validationNormalEyeMovementTrainingDataFileNames = [
    "96",
    "97",
    "98",
    "99",
    "100"
    ]

# Model saving path, and name.
_modelSavingDataPath = "D:\My Data\Tensorflow Models\\"
_modelSavingName = "Blinking Model"





#%%#########################
#   - FUNCTION DEFINES -   #
############################
#%%###################
#   - Read Files -   #
######################
# Opens the CSV files containing the EMG recordings, and combines them into a single list.
# For each sample, a new element for the marker list is also added.
def read_files(path, fileNames, markerType):
    combinedDataList = numpy.empty((0, 2)).astype(float)
    markers = numpy.empty((0), str)
    
    for fileName in tqdm(fileNames):
        try:
            dataFile = open(path + fileName + ".csv", "r", newline = '')            
            reader = csv.reader(dataFile, delimiter = ",")    
            
            dataList = list(reader)
            dataList = clean_data(dataList)
            
            combinedDataList = numpy.append(combinedDataList, numpy.array(dataList).astype(float), axis = 0)
        finally:
            dataFile.close()
        
    for i in range(0, len(combinedDataList)):
        markers = numpy.append(markers, markerType)
        
    print(f"Data: {len(combinedDataList)}")
    print(f"Markers: {len(markers)}")
        
    return combinedDataList, markers


#%%###########################
#   - Encode Marker Data -   #
##############################
# Encodes the values within the marker list to an interger list.
def encode_string_list(stringList):   
    encoder = LabelEncoder()        
    return encoder.fit_transform(stringList)


#%%################################
#   - Clean Data For Training -   #
###################################
# Removes a group of indexes from the start of each data set.
# This is done to avoid the miscaricterisation of the data for non-blinks (at the start of a blink) to be considered a blink.
def clean_data(dataList):
    indexesToRemove = []
    
    for i in range(0, 50):
        indexesToRemove.append(i)
    
    dataList = numpy.delete(dataList, indexesToRemove, axis = 0)      
    return dataList


#%%#########################
#   - Build FFNN Model -   #
############################
# Builds the FFNN hyper parameters model architecture.
# Used for against the search algorythms to find the highest accuracy networks.
def build_model(hp):
    model = keras.Sequential()
    
    #Input Layer
    model.add(layers.Flatten(input_shape=(2,)))    
    
    #Hidden Layers
    for i in range(hp.Int('Layers', 1, 6)):
        hidden_layer_hp_units = hp.Int(f'Neurons: {i}', min_value = 4, max_value = 512, step = 4)
        hidden_layer_hp_activation = hp.Choice(f"Activation Function: {i}", ['relu', 'sigmoid', 'softmax'])
                                
        model.add(keras.layers.Dense(units = (hidden_layer_hp_units), activation = (hidden_layer_hp_activation)))
    
    #Output Layer
    output_layer_hp_activation = hp.Choice(f"Activation Function: {i}", ['relu', 'sigmoid', 'softmax'])
    model.add(layers.Dense(units = 1, activation = (output_layer_hp_activation)))
    
    hp_learning_rate = hp.Choice('Learning Rate', values=[1e-2, 1e-3, 1e-4])
    
    model.compile(
        optimizer = keras.optimizers.Adam(learning_rate = hp_learning_rate),
        #loss = keras.losses.SparseCategoricalCrossentropy(from_logits = True),
        loss = keras.losses.BinaryCrossentropy(from_logits = True),
        metrics = ['accuracy']
    )

    return model


#%%#####################################
#   - Generate and Train The Model -   #
########################################
# Runs the training on the network for a series of possible networks using Keras Tuner.
# The aim is to find the best network out of many.
def generate_network(trainingDataArray, trainingMarkerArray, validationDataArray, validationMarkerArray):
    tuner = keras_tuner.BayesianOptimization(
         hypermodel = build_model,
         objective = 'val_accuracy',
         max_trials = 50,
         
         directory = 'Model_Generation',
         project_name = 'Blink_Classification',
         overwrite = True
    )

    tuner.search(    
        trainingDataArray, 
        trainingMarkerArray, 
        epochs = 50, 
        validation_data = (validationDataArray, validationMarkerArray),
        callbacks = [tensorflow.keras.callbacks.EarlyStopping('val_loss', patience=3)]
    )

    model = tuner.get_best_models()[0]
    model.summary()
    
    return model


#%%###################
#   - Save Model -   #
######################
# Saves the model to a file, with the name and path supplied as a global variable.
def save_model(model):
    model.save(f"{_modelSavingDataPath}{_modelSavingName}.h5")
    print(f"Data saved to '{_modelSavingDataPath}{_modelSavingName}.h5'")





#%%################
#   - Program -   #
###################
# Loads in a group of files, and cleans and encodes them for usage within a models training.
# Generates the model for EMG based blink classification, and then trains it.
# Writes the outputted network to a file for use later.
for _i in range(1, 96):
    _normalEyeMovementTrainingDataFileNames.append(str(_i))
    _blinkingTrainingDataFileNames.append(str(_i))

fullDataArray = numpy.empty((0, 2)).astype(float)
fullMarkerArray = numpy.empty((0), str)

_dataArray, _markerArray = read_files(_normalEyeMovementTrainingDataPath, _normalEyeMovementTrainingDataFileNames, "normalEyeMovement")
fullDataArray = numpy.append(fullDataArray, _dataArray, 0)
fullMarkerArray = numpy.append(fullMarkerArray, _markerArray, 0)

_dataArray, _markerArray = read_files(_blinkingTrainingDataPath, _blinkingTrainingDataFileNames, "blinking")
fullDataArray = numpy.append(fullDataArray, _dataArray, 0)
fullMarkerArray = numpy.append(fullMarkerArray, _markerArray, 0)



fullValidationDataArray = numpy.empty((0, 2)).astype(float)
fullValidationMarkerArray = numpy.empty((0), str)

_dataArray, _markerArray = read_files(_normalEyeMovementTrainingDataPath, _validationNormalEyeMovementTrainingDataFileNames, "normalEyeMovement")
fullValidationDataArray = numpy.append(fullValidationDataArray, _dataArray, 0)
fullValidationMarkerArray = numpy.append(fullValidationMarkerArray, _markerArray, 0)

_dataArray, _markerArray = read_files(_blinkingTrainingDataPath, _validationBlinkingTrainingDataFileNames, "blinking")
fullValidationDataArray = numpy.append(fullValidationDataArray, _dataArray, 0)
fullValidationMarkerArray = numpy.append(fullValidationMarkerArray, _markerArray, 0)



_encodedMarkerList = encode_string_list(fullMarkerArray)
_encodedValidationMarkerList = encode_string_list(fullValidationMarkerArray)
       


model = generate_network(fullDataArray, _encodedMarkerList, fullValidationDataArray, _encodedValidationMarkerList)
save_model(model)











