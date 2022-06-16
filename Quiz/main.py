# Pythonscript for little quiz game
# Version 2 13.08.2017

# for GUI development
from kivy.app import App
from kivy.uix.button import Button
from kivy.uix.gridlayout import GridLayout
from kivy.uix.label import Label       
from kivy.core.window import Window   
        
class TestApp(App):  
    
    layout = GridLayout(cols=5)
    buttons = []
             
    def ButtonPressed(self, instance, value):
        instance.background_color = [0,0,0,0]
        
    def DoublePoints(self, *args):
        key = args[3]
        if key == 'd':
            for i in range(len(self.buttons)):
                self.buttons[i].text = str(int(self.buttons[i].text) * 2)
        elif key == 'h':
            for i in range(len(self.buttons)):
                self.buttons[i].text = str(int(int(self.buttons[i].text) * 0.5))
      
    def build(self):
        Window.bind(on_key_down=self.DoublePoints)        
        
        self.title = 'Nerdquiz'             
        
        numberOfCol = 5
        numberOfRows = 5

        pointsByRow = [100, 200, 300, 600, 1000]        
        
        label1 = Label(text="Anime") 
        label2 = Label(text="Kids of the 90s")
        label3 = Label(text="Sci-Fi")
        label4 = Label(text="Videogames")               
        label5 = Label(text="The Best of Both Worlds") 
        
        color1 = [0,1,0,1]
        color2 = [1,0,0,1]
        color3 = [0,0,1,1]
        color4 = [1,1,0,1]
        color5 = [0,1,1,1]
        
        colors = [color1, color2, color3, color4, color5]
        
        # fill buttons
        for i in range(numberOfRows):
            for j in range(numberOfCol):
                buttonToAdd = Button(text=str(pointsByRow[i]), background_color=colors[j])
                buttonToAdd.bind(state=self.ButtonPressed)                
                self.buttons.append(buttonToAdd)

        self.layout.add_widget(label1)
        self.layout.add_widget(label2)
        self.layout.add_widget(label3)
        self.layout.add_widget(label4)
        self.layout.add_widget(label5)  
        
        for but in self.buttons:
            self.layout.add_widget(but)            
        
        return self.layout

if __name__ == '__main__':
    TestApp().run()
