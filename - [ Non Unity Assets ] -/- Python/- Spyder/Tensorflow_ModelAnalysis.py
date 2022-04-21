from tensorflow import keras
import numpy
import csv
from sklearn.preprocessing import LabelEncoder
from tqdm import tqdm

#%%########################
#   - Clear Console -     #
###########################
print("\033[H\033[J") 





#%%###########################
#   - Global Variables -     #
##############################
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




#%%################################
#   - Read in Training Data -     #
###################################
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





#%%#####################
#   - Load Model -     #
########################
print("\n - Reading in model. \n")
model = keras.models.load_model(modelFullPath)




#%%###################################
#   - Encode Markers into ID's -     #
######################################
print("\n - Encode the markers into unique ID's. \n")

encoder = LabelEncoder()

markerIDArray = encoder.fit_transform(fullMarkerArray)
print(markerIDArray)

print("Data List Size: " + str(len(fullDataArray)))
print("Marker List Size: " + str(len(markerIDArray)))





#%%#####################
#   - Prediction -     #
########################
print("\n - Predicting the network output. \n")  

#print("Proberbility Prediction")
#print(model.predict_proba(fullDataArray))  

print("Evaluated Score")
evaluationScore = model.evaluate(fullDataArray, markerIDArray, verbose=1);
print("Loss: " + str(evaluationScore[0]) + ", Accuracy: " + str(evaluationScore[1]))





#%%#############################
#   - Save The Model -     #
################################
print("\n - Saving the tensorflow model. \n")

modelName = "MotorImageryModel"
model.save('D:\My Data\Tensorflow Models\\' + modelName + '.h5')
print("Data saved to 'D:\My Data\Tensorflow Models\\" + modelName + ".h5'")