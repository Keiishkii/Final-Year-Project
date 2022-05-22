import pyedflib
import csv
import numpy
from tqdm import tqdm

#%%########################
#   - Clear Console -     #
###########################
print("\033[H\033[J") 





#%%###########################
#   - Global Variables -     #
##############################
inputDataPath = "D:\My Data\[ EMG Data ]\[ EDF files ]\\"
outputDataPath = "D:\My Data\[ EMG Data ]\[ EDF files ]\\"

dataFileNames = [
    "S001E01",
    "S001E02",
    "S001E03",
    "S001E04",
    "S002E01",
    "S002E02",
    "S002E03",
    "S002E04",
    "S003E01",
    "S003E02",
    "S003E03",
    "S003E04",
    "S004E01",
    "S004E02",
    "S004E03",
    "S004E04",
    ]


#%%#######################
#   - Opening File -     #
##########################
def open_file(fileName):
    print("\n - Reading in the file. \n")    
    
    return pyedflib.EdfReader(inputDataPath + fileName + ".edf")
 
    
#%%##############################
#   - Organising EEG Data -     #
#################################   
def organise_data(reader):
    print("\n - Organising EEG data from file. \n")
    
    signalCount = reader.signals_in_file
    signalLabels = reader.getSignalLabels()
    
    signalBuffer = numpy.zeros((signalCount, reader.getNSamples()[0]))
    for i in numpy.arange(signalCount):
        signalBuffer[i, :] = reader.readSignal(i)
      
    transposedData = signalBuffer.transpose();
    
    return signalLabels, transposedData


#%%#############################
#   - Organising Markers -     #
################################
def organise_markers(reader, transposedData):
    print("\n - Adding markers to the CSV. \n")
    
    signalAnnotations = reader.readAnnotations()
    
    print("Annotations: " + str(signalAnnotations))
    
    
    sensorFrequency = reader.getSampleFrequency(0)
    sampleDuration = 1 / sensorFrequency
    
    markerIndex = 0
    markerArray = []
    
    if signalAnnotations[2].size > 0:
        marker = signalAnnotations[2][markerIndex]
    
        timeElapsed = 0
        for i in range(len(transposedData)):   
            if markerIndex + 1 < signalAnnotations[0].size and timeElapsed > signalAnnotations[0][markerIndex + 1]:
                markerIndex += 1
                marker = signalAnnotations[2][markerIndex]
            
            timeElapsed += sampleDuration    
            markerArray.append([marker])
        
    return markerArray
            
    
#%%#########################
#   - Writing to CSV -     #
############################
def write_data_to_csv(fileName, signalLabels, transposedData):
    print("\n - Writing EEG data to CSV. \n")
    
    csvFile = open(outputDataPath + fileName + ".csv", "w", newline = '')
    
    writer = csv.writer(csvFile)
    writer.writerow(signalLabels)
    
    for i in transposedData:
        writer.writerow(i)
    
    
    
def write_markers_to_csv(fileName, markerArray):
    print("\n - Writing EEG markers to CSV. \n")
    csvFile = open(outputDataPath + fileName + "_Markers.csv", "w", newline = '')
    
    writer = csv.writer(csvFile)
    writer.writerow(["Markers"])
    
    for i in markerArray:
        writer.writerow(i)
    
    
#%%#######################
#   - Closing File -     #
##########################
def close_file(reader):
    print("\n - Closing file. \n")
    
    reader.close()
    
    
#%%##################
#   - Program -     #
#####################
for fileName in tqdm(dataFileNames):
    reader = None
    try:
        reader = open_file(fileName)     
        
        signalLabels, transposedData = organise_data(reader)
        markerArray = organise_markers(reader, transposedData)
        
        write_data_to_csv(fileName, signalLabels, transposedData)
        
        if len(markerArray) > 0:
            write_markers_to_csv(fileName, markerArray)
    except:
        print("error")
    else:
        close_file(reader)
    
    

