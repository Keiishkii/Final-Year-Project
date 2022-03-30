import pyedflib
import csv
import numpy

path = "D:\My Data\EEG Data\S001R14"





#%%#######################
#   - Opening File -     #
##########################
print("\n - Reading in the file. \n")

reader = pyedflib.EdfReader(path + ".edf")





#%%##############################
#   - Organising EEG Data -     #
#################################
print("\n - Organising EEG data from file. \n")

signalCount = reader.signals_in_file
signalLabels = reader.getSignalLabels()

signalBuffer = numpy.zeros((signalCount, reader.getNSamples()[0]))
for i in numpy.arange(signalCount):
    signalBuffer[i, :] = reader.readSignal(i)
  
transposedData = signalBuffer.transpose();





#%%#############################
#   - Organising Markers -     #
################################
print("\n - Adding markers to the CSV. \n")

signalAnnotations = reader.readAnnotations()
sensorFrequency = reader.getSampleFrequency(0)
sampleDuration = 1 / sensorFrequency


markerIndex = 0
marker = signalAnnotations[2][markerIndex]

timeElapsed = 0
markerArray = []
for i in range(len(transposedData)):   
    if markerIndex + 1 < signalAnnotations[0].size and timeElapsed > signalAnnotations[0][markerIndex + 1]:
        markerIndex += 1
        marker = signalAnnotations[2][markerIndex]
    
    timeElapsed += sampleDuration    
    markerArray.append([marker])
        




    
#%%#########################
#   - Writing to CSV -     #
############################
print("\n - Writing EEG data to CSV. \n")

csvFile = open(path + ".csv", "w", newline = '')

writer = csv.writer(csvFile)
writer.writerow(signalLabels)

for i in transposedData:
    writer.writerow(i)



print("\n - Writing EEG markers to CSV. \n")
csvFile = open(path + "_Markers.csv", "w", newline = '')

writer = csv.writer(csvFile)
writer.writerow(["Markers"])

for i in markerArray:
    writer.writerow(i)



    
    
#%%#######################
#   - Closing File -     #
##########################
print("\n - Closing file. \n")

reader.close()





