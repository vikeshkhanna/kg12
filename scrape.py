import urllib
import math
import os
import re
from bs4 import BeautifulSoup

MAX_DAYS=84
f = open("days.txt","w")

f.write("<workout>")

for day in range(1,MAX_DAYS+1):
	
	f.write("<day>\n")
	f.write("<num>" + str(day) + "</num>\n")
		
	try:
		week = int(math.ceil(day/7.0))
		url = "http://www.bodybuilding.com/fun/kris-gethin-12-week-daily-trainer-week-%s-day-%s.html"%(str(week), str(day))
		print("Opening URL : %s\nPlease wait..."%url)
		site = urllib.urlopen(url)
		html =  site.read()
		soup = BeautifulSoup(html)
		
		f.write("<workout>")
		workout_element = soup.find("h2", {"class":"article-sub-header"}).parent
		workout_name = workout_element.text.split("-")[0].split(":")[-1].strip()
		f.write(workout_name.replace("&","and"))
		f.write("</workout>")
		
		head = soup.find("div",{"class":"workout-header-blue"}).parent
		lis = head.find_all("li")
		
		f.write("<exercises>")
		for item in lis:
			exercise = item.h4.a.text
			f.write("<exercise>")
			f.write("<name>" + exercise + "</name>\n")
			item.h4.extract()
			contents = item.find("span",{"class":"mpt-content"}).contents
			description = "".join(str(x) for x in contents).strip().replace("<br/>","\n")
			f.write("<description>" + description + "</description>\n")
			f.write("</exercise>\n")
			images = item.find_all("img")
			#print("Getting images. Please wait...")
			
			for i in range(len(images)):
				filename = exercise.lower().replace(" ","_") + "_" + str(i) + ".jpg";
				outpath = os.path.join(os.path.join(os.getcwd(), "pics"), filename)
				#urllib.urlretrieve(images[i]["src"], outpath)
				#print("Images saved to " + filename)
		
		f.write("</exercises>")
		
			#print exercise, description
	except:
			print("Exception occured. Ignoring")
			
	f.write("</day>\n")

f.write("</workout>")
