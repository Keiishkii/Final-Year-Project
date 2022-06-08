import tensorflow
from tensorflow import keras
from tensorflow.python.keras.callbacks import TensorBoard
from time import time
import numpy

#%%######################
#   - Clear Console -   #
#########################
# Clears the console on starting the python program.
print("\033[H\033[J") 





#%%#########################
#   - Global Variables -   #
############################
# Global variables for the program, used to train the XOR model.
inputValues = numpy.array([[0,0],[0,1],[1,0],[1,1]])
inputResults = numpy.array([[0],[1],[1],[0]])





#%%#########################
#   - FUNCTION DEFINES -   #
############################
#%%#######################
#   - Generate Model -   #
##########################
# Using Keras generates a feed forward neural network in the shape:
#   2 -> 50 -> 2
def generate_model(): 
    model = keras.Sequential([
        keras.layers.Flatten(input_shape=(2,)),
        keras.layers.Dense(units=50, activation=("sigmoid")),
        keras.layers.Dense(units=2, activation=("sigmoid"))
        ])
        
    tensorboard = TensorBoard(log_dir=("logs/{}".format(time())))
    model.compile(loss = keras.losses.SparseCategoricalCrossentropy(), optimizer = tensorflow.optimizers.Adam(), metrics=['accuracy'])
    
    model.fit(inputValues, inputResults, epochs=25000, callbacks=([tensorboard]))
    model.summary()
    
    return model

#%%###############################
#   - Print Model Prediction -   #
##################################
# prints the output prediction of the model.
def print_model_prediction(model):
    print(model.predict_proba(inputValues))


#%%###################
#   - Save Model -   #
######################
# Saves the model to a file at the directory given.
# This is a .h5 file, which is the second type of saving techneque used by Keras.
def save_model(model):
    model.save('D:\My Data\Tensorflow Models\XOR.h5')
    print("Data saved to 'D:\My Data\Tensorflow Models\XOR.h5'")





#%%################
#   - Program -   #
###################
# Create a model using simple XOR input data, that can be used to classify the the data without the need for logic gates.
# Prints and then saves this model to a file.
_model = generate_model()

print_model_prediction(_model)
save_model(_model)




