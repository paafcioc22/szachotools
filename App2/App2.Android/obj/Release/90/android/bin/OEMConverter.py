from Tkinter import *
import tkMessageBox
import string, thread, time, os, sys

from tempfile import mkstemp
from shutil import move
from os import remove, close

languages = [
    ("CITIZEN",1),
    ("SOURCETECH",2),
    ("GENERIC",3),
]

dir_citizen = "CMP"
dir_sourcetech = "ST"
dir_generic = "GN"

str_sewoo = ["sewoo","LK_"]
str_citizen = ["citizen","CMP_"]
str_sourcetech = ["sourcetech","ST_"]
str_generic = ["andprn",""]

def fileCopy(file_path, abs_path):
	new_file = open(abs_path,'w')
	old_file = open(file_path	)
	for line in old_file:
		for index in range(len(str_sewoo)):
			line = line.replace(str_sewoo[index], str_generic[index])
		new_file.write(line)
	new_file.close()
	old_file.close()
	
def fileRead():
	dirRead(".\src")

def changeFileName(fname):
	selected = v.get()
	if selected == 1:
		dir = dir.replace(str_sewoo[0],str_citizen[0])
		dir = dir.replace("src","src_C")
		sc = "src_C"
	elif selected == 2:
		dir = dir.replace(str_sewoo[0],str_sourcetech[0])
		dir = dir.replace("src","src_S")
		sc = "src_S"
	else:
		dir = dir.replace(str_sewoo[0],str_generic[0])
		dir = dir.replace("src","src_G")
		sc = "src_G"
	
# change directory name, make directory if it is not exists.
def changeDir(dir):
	selected = v.get()
	if selected == 1:
		dir = dir.replace(str_sewoo[0],str_citizen[0])
		dir = dir.replace("src","src_C")
		sc = "src_C"
	elif selected == 2:
		dir = dir.replace(str_sewoo[0],str_sourcetech[0])
		dir = dir.replace("src","src_S")
		sc = "src_S"
	else:
		dir = dir.replace(str_sewoo[0],str_generic[0])
		dir = dir.replace("src","src_G")
		sc = "src_G"
	
	# src directory
	if not os.path.exists(sc):
		os.mkdir(sc)
	
	if not os.path.exists(dir):
		os.mkdir(dir)
	return dir
		
def dirRead(currentDir):
	for root, dirs, files in os.walk(currentDir): # Walk directory tree
		for f in files:
			#print root+" "+f
			#print dirs
			print "=========================="
			print root+"\\"+f+" to "+changeDir(root)+"\\"+f
			print "=========================="			
			fileCopy(root+"\\"+f,changeDir(root)+"\\"+f)
			#print changeDir(root)
			#infos.append(FileInfo(f,root))
	
	
	
def ShowChoice():
	selected = v.get()
	#e1.delete(0,END)
	#if selected == 1:
	#	e1.insert(0,"COM1:9600")
	#elif selected == 2:
	#	e1.insert(0,"LPT1")
	#elif selected == 3:
	#	e1.insert(0,"USB")
	#elif selected == 4:
	#	e1.insert(0,"192.168.0.192")
	print 'Interface '+str(selected)
	
def loops():
	thread.start_new_thread(SamplePressed,())
	
def stops():
	os.popen("kill -9 "+str(t2))
	
def ExitPressed():
	run = False
	ClosePressed()
	
	
# main
root = Tk()
root.wm_title("OEM Converter")
root.config(width=1000, height=500)
v = IntVar()
v.set(3)  # initializing the choice, i.e. Python
run = True


# Port Entry
#e1 = Entry(root)
#e1.grid(row=5, column=0)

# CITIZEN, GENERIC 
for txt, val in languages:
    Radiobutton(root, 
                text=txt,
                padx = 20, 
                variable=v, 
                command=ShowChoice,
                value=val).grid(row=val, column=0)

# Open Button
Button1 = Button(root,text='Convert',command=fileRead)
Button1.grid(row=6, column=0,sticky=W+E)
				
# set Default
#ShowChoice()
mainloop()