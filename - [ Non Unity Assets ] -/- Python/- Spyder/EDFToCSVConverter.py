import pyedflib
import csv
import numpy
from tqdm import tqdm

#%%########################
#   - Clear Console -     #
###########################
# Clears the console on starting the python program.
print("\033[H\033[J") 





#%%###########################
#   - Global Variables -     #
##############################
# Global variables indecating the reading and writing paths for the program.
# These the input paths are used for the edf files and the ouput paths are where the csv's are written too. 
inputDataPath = "D:\My Data\[ EMG Data ]\[ EDF files ]\\"
outputDataPath = "D:\My Data\[ EMG Data ]\[ CSV files ]\\"

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





#%%#########################
#   - FUNCTION DEFINES -   #
############################
#%%#####################
#   - Opening File -   #
########################
# Opens the file using the EDF Reader package.
def open_file(fileName):
    print("\n - Reading in the file. \n")    
    
    return pyedflib.EdfReader(inputDataPath + fileName + ".edf")
 
    
#%%############################
#   - Organising EEG Data -   #
###############################   
# Attempts to unpack the EDF file to gain access to the data recorded data streams.
# On doing so stores and the returns this as a list.
def organise_data(reader):
    print("\n - Organising EEG data from file. \n")
    
    signalCount = reader.signals_in_file
    signalLabels = reader.getSignalLabels()
    
    signalBuffer = numpy.zeros((signalCount, reader.getNSamples()[0]))
    for i in numpy.arange(signalCount):
        signalBuffer[i, :] = reader.readSignal(i)
      
    transposedData = signalBuffer.transpose();
    
    return signalLabels, transposedData


#%%###########################
#   - Organising Markers -   #
##############################
# Attempts to upack the EDF files to gain access to the markers stored within.
# In doing so will return a list of markers, one for each of the samples given.
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
            
    
#%%#######################
#   - Writing to CSV -   #
##########################
# Writes the recorded data to ta CSV file, where each colomn is a channel, and each row is a sample.
def write_data_to_csv(fileName, signalLabels, transposedData):
    print("\n - Writing EEG data to CSV. \n")
    
    csvFile = open(outputDataPath + fileName + ".csv", "w", newline = '')
    
    writer = csv.writer(csvFile)
    writer.writerow(signalLabels)
    
    for i in transposedData:
        writer.writerow(i)
    
    
# Writes the marker list to a CSV filed with the colomn heading of 'Markers'
def write_markers_to_csv(fileName, markerArray):
    print("\n - Writing EEG markers to CSV. \n")
    csvFile = open(outputDataPath + fileName + "_Markers.csv", "w", newline = '')
    
    writer = csv.writer(csvFile)
    writer.writerow(["Markers"])
    
    for i in markerArray:
        writer.writerow(i)
    
    
#%%#####################
#   - Closing File -   #
########################
# Closes the file
def close_file(reader):
    print("\n - Closing file. \n")
    
    reader.close()
    
    
    
    
    
#%%################
#   - Program -   #
###################
# A program for reading in and converting an EDF file for offline data to a CSV file.
# Itterates over a series of EDF files an then unpacks there data into lists.
# Using these lists the program then rewrites them as CSV's
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
    
    

