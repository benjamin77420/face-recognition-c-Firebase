# face-recognition-c#-Firebase-
Hello to whom it may interest :-)

This is a little program that connects to a Firebase client and reads all the "Workers" that are sored in it(all the sub folders that are present).
For every sb folder that is found a new folder holding the name equals to the value that is in the ID filed fin that same sub folder, after the creation of that folder all the imagws that are stored there in base 64 will be converted to jpg images and will be stored in the newly created folder, all the images will be called by the value of the FullName filed and value of the ID filed, for example:

         | |
         | |
         \ /
         
77420
\|
\|
---> FullName : "Benjamin Ben-David", 
      ID : "77420", 
      faceImg1 : ....., 
      faceImg2 : .....

         | |
         | |
         \ /
         
Benjamin Ben-David_77420_0.jpg, 
Benjamin Ben-David_77420_1.jpg, 
Benjamin Ben-David_77420_2.jpg

after the creation of the main training folder the program will make an array that is made of all the paths to files that end with a .jpg( *.jpg ).
from there the recognizer will be "feed" with all the img and it will start to its job.

please make sure that the used Firebase will sher the same fileds value OR you can always change and recode the respected fileds.

Guys if you find your self having some problems with the code you are more then welcome to email my freely, hope it wont come to this :-).

📧bendavidbenyamin@gmail.com

Best regards,
Ben-David Benjamin

