# LIBRARIES 
import pandas as pd
import seaborn as sb
import matplotlib.pyplot as plt
import numpy as np
import os

#------------------------------------------------------------------------------------------------------------------------------------------------------------------
#ABSOLUTE PATH FOR EXCEL/CSV FILES

dirName= os.path.dirname(__file__)
absFileName = os.path.join(dirName, r'B104_Project_Data.csv')
df = pd.read_csv(absFileName)

#renaming columns
df.rename(
    columns={
        "q1": "age", "q2": "sex", "q3": "grade", "q4": "H or L", "q5": "race", "q6": "height", "q7": "weight",
        "q8": "seatbelt", "q16": "fights"},
    inplace=True
    )


#dropna is killing some variables because I think it is erasing all rows with a null value
# df = pd.read_csv('E:/python/B104_project.csv')
# df = df.dropna()
print(df)

#------------------------------------------------------------------------------------------------------------------------------------------------------------------
# DATA FRAMES I
# This counts the answers of the questions and how many people submitted these answers
# Orignal data frames 
v1 =print(df['age'].value_counts())
v2 =print(df['sex'].value_counts())
v3 =print(df['grade'].value_counts())
v4 =print(df['H or L'].value_counts())
v5 =print(df['race'].value_counts())
v6 =print(df['height'].value_counts())
v7 =print(df['weight'].value_counts())
v8 =print(df['seatbelt'].value_counts())
v16 =print(df['fights'].value_counts())

#------------------------------------------------------------------------------------------------------------------------------------------------------------------
# DATA FRAMES II
# Revamped data frame code so we can call the variable for each data frame.
age = (df['age'].tolist())
sex = df['sex'].tolist()
grade = df['grade'].tolist()
hislat = df['H or L'].tolist()
race = df['race'].tolist()
height = df['height'].tolist()
weight = df['weight'].tolist()
seatbelt = df['seatbelt'].tolist()
fights = df['fights'].tolist()

#------------------------------------------------------------------------------------------------------------------------------------------------------------------
# Notes for class
# Male vs Female Heatmap
# Avarge bmi
# Loop through age and see how many responses are 1



# Dead code values just for easy visualization
# age12, age13, age14, age15, age16, age17, age18 = [20],[46],[2699],[3571],[3576],[3303],[904]
# male, female = [7318],[6667]
# grade9, grade10, grade11, grade12, other = [3746],[3578],[3491],[3284],[17]
# never, rarely, sometimes, most_of_the_time, always = [265],[512],[1109],[3253],[7638]
# times_0,time_1,times_2_3,times_4_5,times_6_7,times_8_9,times_10_11,times_12_or_more =[8572],[902],[683],[202],[70],[36],[26],[122]

#------------------------------------------------------------------------------------------------------------------------------------------------------------------
# Originally was going to use this code to replace weight and height because we could not get the Heatmap working
# After getting the Heatmap working we decided to keep the code as dead code just because we spent time on it
# def BMI():
   
# Calculation to find averages of height and weight and convert them into imperial measurements
#     avg_weight_metric=float(df['weight'].mean())
#     avg_height_metric=float(df['height'].mean())
#     # avg_height_metric=print(df['height'].mean())
#     print(avg_weight_metric)
#     print(avg_height_metric)
   
#     avg_weight_freedom=float(avg_weight_metric * 2.2)
#     avg_height_freedom=float(avg_height_metric *  3.28084)
#     print(avg_weight_freedom)
#     print(avg_height_freedom)


# BMI()

#------------------------------------------------------------------------------------------------------------------------------------------------------------------
# BARCHART CODE
# Counting how many people by their grade and sex, there is mild descrepencys where
# Numbers just barely dont add up, going to assume that is because of a null value of some sort
# Because of how little of a number the sum is off by, I believe this is because we are not using the drop.na
def Bar():
    fem9 = ((df['grade'] == 1) & (df['sex'] == 1)).sum()
    fem10= ((df['grade'] == 2) & (df['sex'] == 1)).sum()
    fem11= ((df['grade'] == 3) & (df['sex'] == 1)).sum()
    fem12= ((df['grade'] == 4) & (df['sex'] == 1)).sum()
    femother= ((df['grade'] == 5) & (df['sex'] == 1)).sum()
   
    male9= ((df['grade'] == 1) & (df['sex'] == 2)).sum()
    male10= ((df['grade'] == 2) & (df['sex'] == 2)).sum()
    male11= ((df['grade'] == 3) & (df['sex'] == 2)).sum()
    male12= ((df['grade'] == 4) & (df['sex'] == 2)).sum()
    maleother= ((df['grade'] == 5) & (df['sex'] == 2)).sum()
       
       
       
# Bar graph showing the gender of each grade group
    x= np.arange(5)
    plt.xlabel("Class Grade")
    plt.ylabel("Participants")
    plt.title("Number of Participants and their Class Grade")
    plt.xticks(x, ['Ninth', 'Tenth', 'Eleventh', 'Twelth', 'Other'])
   
    y1 = ([fem9, fem10, fem11, fem12, femother])
# Female=y1
    y2 = ([male9, male10, male11, male12, maleother] )
    width = 0.40
         
     
    plt.bar(x-0.2, y1, width, color='red')
    plt.bar(x+0.2, y2, width, color='salmon')
   
    colors = {'Female':'red', 'Male':'salmon'}        
    labels = list(colors.keys())
    handles = [plt.Rectangle((0,0),1,1, color=colors[label]) for label in labels]
    plt.legend(handles, labels)
    plt.show()

#------------------------------------------------------------------------------------------------------------------------------------------------------------------
# PIE CHART CODE FOR FIGHTS
def Pie1():
    y = np.array([8572, 2041])
    # mylabels = ["Not in Fights", "In Fights"]
    boom = [0.05,0.05,]
    
    mycolors = ['salmon','royalblue']
    colors = {'Not in fights, 8572':'salmon', 'In Fights, 2041':'royalblue'}
    labels = list(colors.keys())
    handles = [plt.Rectangle((0,0), 1,1, color = colors [label]) for label in labels]
    plt.pie(y, startangle = 180, explode = boom, shadow = True,colors= mycolors)
    plt.legend(handles, labels, bbox_to_anchor = (1, 0.5), loc = 'lower left', title = 'Physical Altercations')
    plt.show()

#------------------------------------------------------------------------------------------------------------------------------------------------------------------
# PIE CHART CODE FOR ALL SEATBELT ANSWERS
def Pie2():
    y = np.array([7638,3253, 1109, 512, 265])
    # mylabels = ["Always Wears seatbelt", "Most of the Time wears Seatbelt", "Sometimes wears Seatbelt", "Rarely Wears Seatbelt", "Never Wears Seatbelt"]
    mycolors = ['navy','mediumaquamarine','salmon','lightcyan','darkmagenta']
    colors = {'Always, 7638':'navy', 'Most of the Time, 3253':'mediumaquamarine','Sometimes, 1109':'salmon', 'Rarely, 512':'lightcyan','Never, 265': 'darkmagenta'}
    labels = list(colors.keys())
    handles = [plt.Rectangle((0,0),1,1, color=colors[label]) for label in labels]
    plt.pie(y,startangle = 180, colors= mycolors, shadow =True)
    plt.legend(handles, labels,bbox_to_anchor=(1, 0.5), loc='lower left',title = "Seatbelt Usage")
    plt.show()
    
#------------------------------------------------------------------------------------------------------------------------------------------------------------------
# HEATMAP CODE
# Plotting correlation heatmap 
def Heatmap():
    df_heat = df.drop(columns=["race"]) 
    plt.rcParams.update({'font.size':18})
    plt.rcParams['figure.facecolor'] = 'xkcd:white'
    plt.figure(figsize = (12, 7), dpi = 200)
    sb.heatmap(df_heat.corr(), annot = True, vmin = 0, vmax = 1,cmap= "magma")
    plt.show()
    
#------------------------------------------------------------------------------------------------------------------------------------------------------------------
# SELECTION MENU CODE
# Used while True to keep an infinte loop going so it will be up to the user to decide 
# Try and ValueError to catch letter errors 
# Breaks for going into Yes / No option and ending the menu loop

def display_menu():
        while True:
            print()
            print('--------------------------------------------------------------------------------------------')
            print('\t\t\t\t\t Hello, Welcome to our Project Menu')
            print('\n')
            print('\t\t\t\t\t\t\t 1. Bar Chart')
            print('\n')
            print('\t\t\t\t\t\t\t 2. Fights Pie Chart')
            print('\n')
            print('\t\t\t\t\t\t\t 3. Seatbelts Pie Chart')
            print('\n')
            print('\t\t\t\t\t\t\t 4. Heat Map')
            print('\n')
            print('\t\t\t\t\t\t\t 5. All')
            print('\n')
            print('\t\t\t\t\t\t\t 6. Exit')
            print('\n')
            while True:
                try:
                    user = int(input('\t\t\t\t\t\t\t Please select an option: ')) 
                    print('\n')
                    print('--------------------------------------------------------------------------------------------')
                    if user not in [1, 2, 3, 4, 5, 6]:
                        print('\n')
                        print("\t\t\t\t\t\t\t Sorry, that is not a valid option.") 
                        print("\t\t\t\t\t\t\t Please select a valid option.")
                        print('\n')
                        print('--------------------------------------------------------------------------------------------')
                        print('\n')
                        continue
                    if user == 1:
                        print('\n')
                        print("\t\t\t\t\t\t\t Here is our Barchart. ")
                        print("\t\t\t\t\t\t\t This shows the Grades, Sex, ")
                        print("\t\t\t\t\t\t\t and how many participants. ")
                        print('\n')
                        print('--------------------------------------------------------------------------------------------')
                        Bar()
                    elif user == 2:
                        print('\n')
                        print("\t\t\t\t\t\t\t Here is our Fights Pie Chart ")
                        print("\t\t\t\t\t\t\t This shows the results for often, ")
                        print("\t\t\t\t\t\t\t participants got into fights. ")
                        print('\n')
                        print('--------------------------------------------------------------------------------------------')
                        Pie1()
                    elif user == 3:
                        print('\n')
                        print("\t\t\t\t\t\t\t Seatbelts Pie Chart ")
                        print("\t\t\t\t\t\t\t This shows the results for often, ")
                        print("\t\t\t\t\t\t\t participants wore their seatbelts. ")
                        print('\n')
                        print('--------------------------------------------------------------------------------------------')
                        Pie2()
                    elif user == 4: 
                        print('\n')
                        print("\t\t\t\t\t\t\t Here is our Heat Map. " )
                        print("\t\t\t\t\t\t\t This was used to show the, ")
                        print("\t\t\t\t\t\t\t corrolation of all the data. ")
                        print('\n')
                        print('--------------------------------------------------------------------------------------------')
                        Heatmap()
                    elif user == 5:
                        print('\n')
                        print("\t\t\t\t\t\t\t Here is all of our visuals.") 
                        print("\t\t\t\t\t\t\t For a more detailed analsis of our visuals, ") 
                        print("\t\t\t\t\t\t\t please select an indivual response. ")
                        print('\n')
                        print('--------------------------------------------------------------------------------------------')
                        Bar()
                        Pie1()
                        Pie2()
                        Heatmap()
                    elif user == 6:
                        print('\n')
                        print("\t\t\t\t\t\t\t Thank you for looking at our project. ")
                        print("\t\t\t\t\t\t\t Have a good day :)")
                        return
                    
                    break  # Break out of the option selection loop to return to main menu loop
                except ValueError:
                    print('--------------------------------------------------------------------------------------------')
                    print('\n')
                    print("\t\t\t\t\t\t\t Sorry, that is not a valid option.") 
                    print('\t\t\t\t\t\t\t Please select a valid option.')
                    print('\n')
                    print('--------------------------------------------------------------------------------------------')
            while True:
                print('\n')
                cont = input('\t\t\t\t\t\t\t Would you like to continue? (Y/N): ').upper()
                print('\n')
                if cont not in ["Y", "N"]:
                    print(f'{cont} is not a valid option. Please enter Y or N.')
                else:
                    break
            if cont == 'N':
                print('--------------------------------------------------------------------------------------------')
                print('\n')
                print("\t\t\t\t\t\t\t Thank you for looking at our project. ")
                print("\t\t\t\t\t\t\t Have a good day :)")
                print('\n')
                print('--------------------------------------------------------------------------------------------')
                break

display_menu()

#------------------------------------------------------------------------------------------------------------------------------------------------------------------