![image](https://github.com/user-attachments/assets/6ac5d174-0091-4b02-bc6a-761a39cdbd13)


1.   ![image](https://github.com/user-attachments/assets/009cef34-e67f-4994-9a67-98d293dc7e73)
Enter desired number of nodes and press Node button(5) to populate random distribution of desired number of nodes.

2.   ![image](https://github.com/user-attachments/assets/1688558e-64bb-43cb-987c-23d65e575bba)
Simulation zone is divided in Number of Column, Number of Rows (eg. 6,4) matrix to enable node distribution constrain of uniformly random distribution of nodes with 6 columns and 4 rows. Press Grid (3) to implement and show the grid.

3.  ![image](https://github.com/user-attachments/assets/2de82ac7-ecc5-4cee-ab4f-af0e5539720c)
 Select Grid check box to display grid as per the dimensions (Column, Row) as specified in Zones (2).

4.  ![image](https://github.com/user-attachments/assets/89d41ae8-831b-46f2-9b57-976ed5edbf81)
Press Clear button to clear the nodes deployment. Pressing this will give blank region. 

5.  ![image](https://github.com/user-attachments/assets/ee0c0d2e-7d32-4a93-ac46-004e7c039326)
Press Node button to populate number of nodes as specified in a text-box with label “Nodes”.

6.   ![image](https://github.com/user-attachments/assets/14718e78-75a7-4459-8d55-a6d7f17c5871)
 Press Re-Draw button to re-draw after performing A-Star and Best First Search experiment on selected source and destination nodes. It will re-draw  node deployment, selected source and destination nodes and obstacle. 

7.  ![image](https://github.com/user-attachments/assets/c01c5649-3837-4026-a4b6-1c883faf12e2)
 Press Src-Dst button to randomly select source node and destination node to perform experiment with A-Star and Best First Search algorithm. If obstacle is selected then source node and destination nodes will be randomly selected on two sides of obstacle.

8.  ![image](https://github.com/user-attachments/assets/b2998431-484b-45de-9b55-530f71f7a89e)
 Press A-star button to execute A-Star algorithm on selected source and destination nodes. It plots a path between source and destination.
9.  ![image](https://github.com/user-attachments/assets/fb96d0d9-c0c1-47f8-95b5-e37de3c82488)
 Press BstFst button to execute Best First Search algorithm on selected source and destination nodes. It plots a path between source and destination.

10.  ![image](https://github.com/user-attachments/assets/040103f5-11ef-4e26-8fd9-74ca6cced589)
 Enter the value between 0 and 100 to specify the size of an obstacle as a percentage of the screen when checkbox is selected(11). 

11.  ![image](https://github.com/user-attachments/assets/3180e2d1-d100-42fa-9c05-00c0085db1ad)
 Select the checkbox to draw an obstacle as specified in textbox in (10). An obstacle coordinates are stored in PolyList.txt file.

12. ![image](https://github.com/user-attachments/assets/835b3443-db26-434d-b841-952ac0ba13c9)
 Enter the number of experiments you want to carry out for chosen configuration of nodes. It will start when Save button in (13) is pressed.

13.  ![image](https://github.com/user-attachments/assets/add21888-8808-491d-a51c-b5d0186211ed)
 Press Save button to executed large number of experiments and save its output. It creates two files namely Experiments.txt and Parameters.txt files. 

14. ![image](https://github.com/user-attachments/assets/facbd6fc-fa7c-4f3a-af61-1a1947208baf)
 Press Load button to display saved configuration of nodes and obstacle. It reads Parameters.txt and displays the saved configuration.

15.  ![image](https://github.com/user-attachments/assets/87b717ce-1eda-4f1a-bc8e-0e7127ac5565)
 Press Analysis button to see generated results after Save is pressed. It  displays (source,destination) node pair in combo-box from Experiments.txt file. This pair of nodes are those nodes whose number of intermediate node in A-Star output is less than Best First search output.

16.  ![image](https://github.com/user-attachments/assets/d98ec0c0-ee13-4da9-8b79-fa65e1686d3d)
 Combo-box displays the result of experiment carried out and stored in Experiments.txt file. It displays source, destination node pair when Analysis button is pressed. It displays path between source and destination for both A-Star and Best First Search algorithm when a specific pair is selected from combo-box list. It displays on those pairs in which A-Star executes with fewer intermediate nodes than Best First Search.



 
