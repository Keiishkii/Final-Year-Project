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
# Global variables for the training data of the EEG Motor imagery mode.
# Also the saving path and file name of the model.
trainingDataPath = "D:\My Data\EEG Data\CSV files\\"
trainingDataFileNames = [
    "S001R04",
    "S001R08",
    "S001R12",
    "S002R04",
    "S002R08",
    "S002R12",
    "S003R04",
    "S003R08",
    #"S003R12",
    #"S004R04",
    "S004R08",
    #"S004R12",
    ]

validationDataPath = "D:\My Data\EEG Data\CSV files\\"
validationDataFileNames = [
    #"S001R04",
    #"S001R08",
    #"S001R12",
    #"S002R04",
    #"S002R08",
    #"S002R12",
    #"S003R04",
    #"S003R08",
    "S003R12",
    "S004R04",
    #"S004R08",
    #"S004R12",
    ]

modelSavingDataPath = "D:\- [ Charlie Lloyd-Buckingham ] -\- [ Python - Keras Models ] -\\"
modelSavingName = "Motor Imagery Model"





#%%#########################
#   - FUNCTION DEFINES -   #
############################
#%%####################
#   - Build Model -   #
#######################
# Builds the FFNN hyper parameters model architecture.
# Used for against the search algorythms to find the highest accuracy networks.
def build_model(hp):
    model = keras.Sequential()
    
    #Input Layer
    model.add(layers.Flatten(input_shape=(64,)))    
    
    #Hidden Layers
    for i in range(hp.Int('Layers', 1, 6)):
        hidden_layer_hp_units = hp.Int(f'Neurons: {i}', min_value = 4, max_value = 512, step = 4)
        hidden_layer_hp_activation = hp.Choice(f"Activation Function: {i}", ['relu', 'sigmoid', 'softmax'])
                                
        model.add(keras.layers.Dense(units = (hidden_layer_hp_units), activation = (hidden_layer_hp_activation)))
    
    #Output Layer
    output_layer_hp_activation = hp.Choice(f"Activation Function: {i}", ['relu', 'sigmoid', 'softmax'])
    model.add(layers.Dense(units = 3, activation = (output_layer_hp_activation)))
    
    hp_learning_rate = hp.Choice('Learning Rate', values=[1e-2, 1e-3, 1e-4])
    
    model.compile(
        optimizer = keras.optimizers.Adam(learning_rate = hp_learning_rate),
        loss = keras.losses.SparseCategoricalCrossentropy(from_logits = True),
        metrics = ['accuracy']
    )

    return model


#%%#######################################
#   - Generate Data and Marker Lists -   #
##########################################
# Loads the CSV files and stores there data as lists.
def get_data_and_marker_lists(path, filenames):
    combinedDataList = numpy.empty((0, 64)).astype(float)
    combinedMarkerList = numpy.empty((0), str)
    
    for filename in tqdm(filenames):
        #Open the data and event files
        dataFile = open(path + filename + ".csv", "r", newline = '')
        markerFile = open(path + filename + "_Markers.csv", "r", newline = '')
        try:            
            #reads the data as a csv and formats it into a list of rows
            reader = csv.reader(dataFile, delimiter = ",")    
            dataList = list(reader)
                
            reader = csv.reader(markerFile, delimiter = ",")
            markerList = list(reader)
            
            if len(dataList) != len(markerList): #print the error
                print("\n - - - POTENTIAL ERROR - - - \n" +
                      "File " + filename + ", will not be included due to its inconsistant length" + 
                      " - data length: " + str(len(dataList)) + 
                      " - marker length: " + str(len(markerList)))
                
            else:    
                #removes the first index of the lists, this is the csv headers
                dataList.pop(0)
                markerList.pop(0)    
            
                #adds the contents of the numpyArrays to the full group
                combinedDataList = numpy.append(combinedDataList, numpy.array(dataList).astype(float), axis = 0)
                combinedMarkerList = numpy.append(combinedMarkerList, markerList)    
        
        finally:
            #closes the files
            dataFile.close()
            markerFile.close()    
    
    return combinedDataList, combinedMarkerList


#%%###########################
#   - Encode Marker Data -   #
##############################
# Encodes the values within the marker list to an interger list.
def encode_string_list(stringList):
    encoder = LabelEncoder()
    return encoder.fit_transform(stringList)


#%%###################
#   - Clean Data -   #
######################
# Cleans the data to avoid the misclasification of markers
def clean_data(dataList, markerList):
    if len(markerList) > 0:
        markerIndexes = [markerList[0],]    
        for i in tqdm(range(1, len(markerList))):
            if markerList[i] != markerList[i - 1]:
                markerIndexes.append(i)
            
        markerIndexes.reverse()
        print("Change Count: " + str(len(markerIndexes)))
        
        indexesToRemove = []
        for index in tqdm(markerIndexes): 
            removedCount = 0
            while index + removedCount < len(markerList) and removedCount < 120:
                indexesToRemove.append(index + removedCount)
                removedCount += 1
           
        print ("Removed Index Count: " + str(len(indexesToRemove)))
        markerList = numpy.delete(markerList, indexesToRemove)
        dataList = numpy.delete(dataList, indexesToRemove, axis = 0)     
            
    else:
        print("There is no data")
        
    return dataList, markerList


#%%##############################
#   - Read in Training Data -   #
#################################
# Opens the CSV files containing the EEG samples and markers, then stores them as a list.
def read_in_training_data():
    print("\n - Creating training and validation lists. \n")
    
    print("Creating Training Data Lists.")
    trainingDataList, trainingMarkerList = get_data_and_marker_lists(trainingDataPath, trainingDataFileNames)
    encodedTrainingMarkerList = encode_string_list(trainingMarkerList)
    
    trainingDataList, encodedTrainingMarkerList = clean_data(trainingDataList, encodedTrainingMarkerList)
    
    print("Creating Validation Data Lists.")
    validationDataList, validationMarkerList = get_data_and_marker_lists(validationDataPath, validationDataFileNames)
    encodedValidationMarkerList = encode_string_list(validationMarkerList)
    
    validationDataList, encodedValidationMarkerList = clean_data(validationDataList, encodedValidationMarkerList)

    return trainingDataList, encodedTrainingMarkerList, validationDataList, encodedValidationMarkerList    


#%%#########################
#   - Generate Network -   #
############################
# Runs the training on the network for a series of possible networks using Keras Tuner.
# The aim is to find the best network out of many.
def generate_network(trainingDataList, encodedTrainingMarkerList, validationDataList, encodedValidationMarkerList):
    print("\n - Generating Network. \n")
    
    tuner = keras_tuner.BayesianOptimization(
         hypermodel = build_model,
         objective = 'val_accuracy',
         max_trials = 250,
         
         #num_initial_points = 20
         #alpha = 1e-4,
         #beta = 2.6,
         #seed = 1,
         #tune_new_entries = (True),
         #allow_new_entries = (True),
         
         #distribution_strategy = tensorflow.distribute.MirroredStrategy(),
         
         directory = 'Model_Generation',
         project_name = 'Motor_Imagery_Classification',
         overwrite = True
    )
    
    tuner.search(    
        trainingDataList, 
        encodedTrainingMarkerList, 
        epochs = 5, 
        validation_data = (validationDataList, encodedValidationMarkerList),
        callbacks = [tensorflow.keras.callbacks.EarlyStopping('val_loss', patience=3)]
    )
    
    model = tuner.get_best_models()[0]
    model.summary()

    return model


#%%#######################
#   - Save The Model -   #
##########################
# Saves the FFNN model
def save_network(model):
    print("\n - Saving the tensorflow model. \n")
    
    model.save(f"{modelSavingDataPath}{modelSavingName}.h5")
    print(f"Data saved to '{modelSavingDataPath}{modelSavingName}.h5'")





#%%################
#   - Program -   #
###################
# Loads in CSV files containin EEG data and EEG markers.
# Formats this data to be comparable, and then inserts it as training and validation data for a FFNN, trained using Keras Tuner
# After which, saves the file to the disk.
_trainingDataList, _encodedTrainingMarkerList, _validationDataList, _encodedValidationMarkerList = read_in_training_data()

_model = generate_network(_trainingDataList, _encodedTrainingMarkerList, _validationDataList, _encodedValidationMarkerList)
save_network(_model)


