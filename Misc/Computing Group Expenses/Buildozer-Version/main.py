# Python script for calculating group expenses, where each member spends a different amount of money, but should only
# pay the average.
# Version 2 25.05.2107
import copy
import numpy as np

# for GUI development
from kivy.app import App
from kivy.uix.button import Button
from kivy.uix.textinput import TextInput
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.label import Label
from kivy.core.window import Window   
from kivy.uix.popup import Popup          
        
class TestApp(App):  
    
    layout = BoxLayout(orientation='vertical')
    number_fields = 0
    
    textinput_Number = TextInput(multiline=False)
    text_widgets = []
    
    input_texts_names = []
    input_texts_expenses = []    
    
    introduction_label = Label()
    output_label = TextInput(readonly=True)
    
    # generator to compute permutations of transactions
    def generatePermutation(self, enum = [], permutation = []):
        if len(enum) == 1:
            permutation.append(enum[0])
            yield permutation
    
        for i in enum:
            perm_copy = copy.deepcopy(permutation)
            perm_copy.append(i)
            enum_copy = copy.deepcopy(enum)
            enum_copy.remove(i)
            for perm in self.generatePermutation(enum_copy, perm_copy):
                yield perm                 
    
    # search for best distribution with the min of transactions
    def balanceGroup(self,ind_give, ind_take, min_amount, ls):
        # found, minimal number of transactions
        min_transactions_numb = len(ls) * len(ls)
        min_transactions = []

        # Algorithm
        # first go through permutations of givers
        # second for each giver-permutation go through taker permutations
        # Funny: generate function can not be used directly for small permutations        
        for giver_permutation in self.generatePermutation(ind_give, []):
            for taker_permutation in self.generatePermutation(ind_take, []):   

                print("----------")
                print(giver_permutation)
                print(taker_permutation)
                print("----------")                

                # remove duplicates- essential for under 4 persons
                giver_perm = []
                taker_perm = []
                
                for giver in giver_permutation:
                    if giver not in giver_perm:
                        giver_perm.append(giver)
                        
                for taker in taker_permutation:
                    if taker not in taker_perm:
                        taker_perm.append(taker)
                        
                giver_permutation = giver_perm
                taker_permutation = taker_perm                                
                
                transactions = []
                    
                giver_index = 0                         
                giver = giver_permutation[giver_index]
                giver_amount = (-1)*ls[giver][1]
                
                taker_index = 0                                          
                taker = taker_permutation[taker_index]
                taker_amount = ls[taker][1]
                    
                while True:                
                    # break if it gets worse
                    if len(transactions) >= min_transactions_numb:
                        break
                    # transaction                        
                    if taker_amount < giver_amount:
                        if taker_amount != 0.0:
                            giver_amount -= taker_amount                            
                            transactions.append((giver, taker, taker_amount))                                                    
                            
                        if taker_index < len(taker_permutation) - 1:
                            taker_index += 1
                            taker = taker_permutation[taker_index]
                            taker_amount = ls[taker][1]
                        else:
                            break
                    else:                        
                        if giver_amount != 0.0:
                            taker_amount -= giver_amount
                            transactions.append((giver, taker, giver_amount))                            
                        
                        if giver_index < len(giver_permutation) - 1:
                            giver_index += 1
                            giver = giver_permutation[giver_index]                            
                            giver_amount = (-1)*ls[giver][1]
                        else:
                            break  
                
                if len(transactions) < min_transactions_numb:
                    min_transactions_numb = len(transactions)
                    min_transactions = copy.deepcopy(transactions)                        
                    
        return min_transactions
    
    def computeExpenses(self, csv_list):
        if len(csv_list) <= 1:
            return ""
    
        # cast strings of 'Ausgaben' to real float and to 50 Cent
        for i in range(0, len(csv_list)):
            csv_list[i][1] = float(csv_list[i][1])

        # 1st row = Personname, 2nd row = Rolle, 3rd row = expenses
        # Calculate all expenses
        expenses = sum([csv_list[i][1] for i in range(len(csv_list))])

        # fill last row of entries with the difference between average debt and already paid money, round in the end
        average = np.round(expenses/float(len(csv_list)), 2)
        
        # check if average rounding shades amount of 0.x Cent
        average_not_exact = False
        average_deviation = np.abs(average - expenses/float(len(csv_list)))        
        
        if average_deviation > 0.001:
            average_not_exact = True            

        for i in range(len(csv_list)):
            csv_list[i][1] = np.rint([(csv_list[i][1]-average)*100])[0]

        # make copy of original list
        ls = copy.deepcopy(csv_list)

        # find indices of givers and takers
        ind_give = [i for i in range(len(ls)) if ls[i][1] < 0]
        ind_take = [i for i in range(len(ls)) if ls[i][1] > 0]

        # the fewest amount to be allowed to be transitioned
        min_amount = 1.0
            
        # call function
        min_trans = self.balanceGroup(ind_give, ind_take, min_amount, ls)

        output = ""
        # Show results and important statistics
        output += "Status:\n"
        for i in range(len(csv_list)):
            output += "{0} paid {1:4.2f} EUR\n".format(csv_list[i][0], csv_list[i][1]*0.01+average)
        output += "\nTotal amount of money spent: "+str(expenses)+" EUR\n"
        output +="Average for "+str(len(csv_list))+" people: "+str(average)+" EUR\n"

        if average_not_exact:
            deviation = average_deviation * len(csv_list)
            output += "Average not exact, expected deviation is: "+str(float(np.rint([deviation*100])[0])*0.01)+" EUR\n"
        
        output += "\nCalculated transactions with minimal count of"+str(len(min_trans))+":\n"
        for i in range(len(min_trans)):
            output += "{0} gives {1} {2:4.2f} EUR\n".format(csv_list[min_trans[i][0]][0], csv_list[min_trans[i][1]][0], min_trans[i][2]*0.01)
        
        return output
    
    def Compute(self, instance):    
        self.layout.remove_widget(self.output_label)                                        
        
        list_expenses = []    
        
        for i in range(0,len(self.input_texts_expenses)):
            try:
                name = self.input_texts_names[i].text
                expense = float(self.input_texts_expenses[i].text.replace(',','.'))
            except:
                name = ""
                expense = 0
            if name != "":
                list_expenses.append([name,expense])    
                     
        output = self.computeExpenses(list_expenses)
        self.output_label.text = output
        self.output_label.size_hint = (1, 6)

        # output
        self.layout.add_widget(self.output_label)

        for widget in self.text_widgets:
            self.layout.remove_widget(widget)
                    
        self.layout.do_layout()
                
    def ShowFields(self, instance):
        try:            
            self.number_fields = int(self.textinput_Number.text)
            # warning popup
            if self.number_fields > 10:
                popup = Popup(title='Warning', content=Label(text='Invalid Input'), size_hint=(0.5,0.5))
                popup.open()
                return
        except:
            self.number_fields = 0
            
        # remove introduction label    
        self.layout.remove_widget(self.introduction_label)
        
        # remove possible result label
        self.layout.remove_widget(self.output_label)
        
        for widget in self.text_widgets:
            self.layout.remove_widget(widget)
            
        self.input_texts_expenses = []
        self.input_texts_names = []
        
        text_labels = BoxLayout(orientation='horizontal')
        text_labels.add_widget(Label(text="Name"))
        text_labels.add_widget(Label(text="Expense in Euro"))
        self.text_widgets = [text_labels] 
        self.layout.add_widget(text_labels)              
        
        for i in range(self.number_fields):
            
            text_layout = BoxLayout(orientation='horizontal')
            textinput_name = TextInput(multiline=False)
            textinput_expense = TextInput(multiline=False)
                
            self.input_texts_names.append(textinput_name)
            self.input_texts_expenses.append(textinput_expense)
            
            text_layout.add_widget(textinput_name)
            text_layout.add_widget(textinput_expense)
            self.text_widgets.append(text_layout)
            self.layout.add_widget(text_layout)
                        
        btn_Compute = Button(text='Compute')
        btn_Compute.bind(on_press=self.Compute)
        self.layout.add_widget(btn_Compute)  
        self.text_widgets.append(btn_Compute)               
                        
        self.layout.do_layout()
        
        
    def on_pause(self):
        # Enable pause when minimizing in Android
        return True

    def on_resume(self):
        pass
    
    def build(self):
        self.title = 'Grostifix'
        
        # Problem with resize: Keyboard is over app (resized at the bottom)
        Window.softinput_mode = "below_target"        
        
        self.introduction_label = Label(text="Welcome! Let's balance expenses!", size_hint=(1, 6))                
            
        # Input number of group members to enable textboxes
        btn_InputFields = Button(text='Confirm number of people')
        btn_InputFields.bind(on_press=self.ShowFields)        
        
        number_layout = BoxLayout(orientation='horizontal')
        number_layout.add_widget(self.textinput_Number)
        number_layout.add_widget(btn_InputFields)        
        
        self.layout = BoxLayout(orientation='vertical')
        self.layout.add_widget(self.introduction_label)        
        self.layout.add_widget(number_layout)    
        
        return self.layout

if __name__ == '__main__':
    TestApp().run()
