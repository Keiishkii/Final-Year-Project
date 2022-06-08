from tensorflow import keras
import numpy
import csv
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
# Gives the path of the model and a path for training data as global variables.
modelFullPath = "D:\My Data\Tensorflow Models\MotorImageryModel.h5"

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
    #"S003R12",
    "S004R04",
    "S004R08",
    #"S004R12",
    ]





#%%#########################
#   - FUNCTION DEFINES -   #
############################
#%%#################################
#   - Get Data and Maker Lists -   #
####################################
# Gets the marker data and EEG data from the CSV files, and stors each sample into a 2 dimensional list.
# Removes the first element of the list, as this is used as the heading row within the CSV
def get_data_and_marker_lists():
    print("\n - Reading in training data. \n")
    
    fileCount = 0
    fullDataArray = numpy.empty((0, 64)).astype(float)
    fullMarkerArray = numpy.empty((0), str)
    for fileName in tqdm(validationDataFileNames):
        #Open the data and event files
        dataFile = open(validationDataPath + fileName + ".csv", "r", newline = '')
        markerFile = open(validationDataPath + fileName + "_Markers.csv", "r", newline = '')
    
    
        #reads the data as a csv and formats it into a list of rows
        dataReader = csv.reader(dataFile, delimiter=",")    
        dataList = list(dataReader)
            
        markerReader = csv.reader(markerFile, delimiter=",")
        markerList = list(markerReader)
        
        if len(dataList) != len(markerList):          
            #print the error
            print("\n - - - POTENTIAL ERROR - - - \n")
            print("File " + fileName + ", will not be included due to its inconsistant length")
            print(" - data length: " + str(len(dataList)))
            print(" - marker length: " + str(len(markerList)))
            
        else:    
            #removes the first index of the lists, this is the csv headers
            dataList.pop(0)
            markerList.pop(0)    
        
            #adds the contents of the numpyArrays to the full group
            fullDataArray = numpy.append(fullDataArray, numpy.array(dataList).astype(float), axis = 0)
            fullMarkerArray = numpy.append(fullMarkerArray, markerList)
            fileCount += 1
    
    
        #closes the files
        dataFile.close()
        markerFile.close()
    
    print("Files counted: " + str(fileCount))
    
    return fullDataArray, fullMarkerArray


#%%###################
#   - Load Model -   #
######################
# Loads the model from the disk to be reavaluated.
def load_model():
    print("\n - Reading in model. \n")
    model = keras.models.load_model(modelFullPath)
    
    return model


#%%###########################
#   - Encode Marker Data -   #
##############################
# Encodes the values within the marker list to an interger list.
def encode_string_list(stringList):
    print("\n - Encode the markers into unique ID's. \n")
    
    encoder = LabelEncoder()    
    markerIDArray = encoder.fit_transform(stringList)
    
    return markerIDArray


#%%###################
#   - Prediction -   #
######################
# Using the input data from the files, runs the analysis of the model again to output its accuarcy.
def predict(model, fullDataArray, markerIDArray):
    print("\n - Predicting the network output. \n")  
    
    print("Evaluated Score")
    evaluationScore = model.evaluate(fullDataArray, markerIDArray, verbose=1);
    print("Loss: " + str(evaluationScore[0]) + ", Accuracy: " + str(evaluationScore[1]))





#%%################
#   - Program -   #
###################
# Loads in the model along side a group of data and makers to use as validation.
# Runs the prediction function on the model and outputs the models score.
_model = load_model()
_dataArray, markerArray = get_data_and_marker_lists()

_encodedMarkerArray = encode_string_list(markerArray)

predict(_model, _dataArray, _encodedMarkerArray)





