**A-Star and Best First Search algorithms for Power Efficient WSN Routing**
In this work, we have implemented two heuristic function-based algorithms from the AI domain for power-efficient routing in wireless sensor networks. 
The first algorithm is the A-Star algorithm, and the second is Best-First Search. 
We have considered the following network scenario.

![image](https://github.com/user-attachments/assets/0e592987-7e97-4add-86ef-1ee222c44a11)

**Illustration of a randomly deployed WSN with a source node (S), current node (C), and destination node (D), 
showing possible routing paths from S to C and C to D.**

From this network, a source node (S) and a destination node (D) are randomly selected. Since S and D are not within direct communication range, an optimal path must be established through intermediate nodes (C). To achieve this, a base station (BS) is incorporated to execute the proposed algorithms, determining the most energy-efficient path from each source to its destination within the network. Once the base station computes the optimal path, it disseminates this information to the relevant nodes by embedding the paths in packet headers through flooding. This approach ensures that all nodes along the route are aware of the precomputed optimal path, thereby reducing computational overhead and minimizing energy consumption during real-time data transmission.

The following assumptions are considered for the proposed algorithms.
1.	Each node continuously generates data for transmission, and there is only one base station (BS) in the network.
2.	The network is static and homogeneous, meaning all nodes have identical configurations, capabilities, and remain stationary.
3.	Nodes are randomly deployed, with no possibility of battery replacement or recharging.
4.	Communication occurs over a shared medium, following a standard MAC protocol.
5.	A non-flow splitting model is used, ensuring that data from a source follows a single path to the destination.
6.	The network follows a flat topology, without hierarchical clustering.

* Optimal Data Gathering for Maximized Lifetime: The A-Star algorithm is applied to sensor nodes (1 to m) and a base station (b) to find the most efficient data gathering schedule, maximizing the network's lifetime until the first relay node depletes its power.

* Dynamic Route Management by Base Station: The base station computes and broadcasts the routing schedule for each round, dynamically maintaining N optimal routes for efficient data transmission and energy conservation.

* Cost Function for Path Selection: The distance cost function f(n) = g(n) + h(n) is used, where g(n) represents the path cost from the start to the current node, and h(n) is a heuristic estimate of the remaining distance to the base station.

**Calculation of g(n) and h(n):**
* Admissible Heuristic in Path Estimation: The heuristic function h(n), based on the Euclidean distance, underestimates the actual cost since real paths involve intermediate nodes with varying energy levels, making A-Star's heuristic admissible.

* Tree Structure with OPEN and CLOSED Lists: A-Star maintains a search tree with an OPEN list (priority queue for nodes to be examined) and a CLOSED list (nodes already examined), where each node n tracks f(n) = g(n) + h(n) as an estimate of the best path passing through it.






![image](https://github.com/user-attachments/assets/b9d6e3dc-f084-4c87-ae69-690ef5b5ed28)

**Heuristic-based path selections from Source (S) to Destination (D), with Best-First Search (dotted), 
shortest heuristic path (red), and avoided non-optimal path (dark).**

* Next-Hop Selection in A-Star: Among neighboring nodes, the one with the lowest heuristic cost h(n) (blue node) is chosen as the next hop, ensuring a progressive path selection towards the destination based on estimated shortest distance.

* Key Difference Between A-Star and Best-First Search: Both algorithms use OPEN and CLOSED lists, but A-Star considers total cost f(n) = g(n) + h(n), while Best-First Search uses only heuristic cost f(n) = h(n), ignoring the actual path cost.


**Algorithm Pseudo code**
**Algorithm DoAStar()**    
    Input: Network nodes, connectivity information, battery thresholds
    Output: Established paths and network lifetime in rounds

    for a ← 0 to 1 do  /* Perform routing twice for bidirectional 
                          evaluation */
        node2dst ← GetNodeSrtdDistList(nod2dNew, dstNo)  
        /* Generate a sorted list of nodes based on their distance to 
           the destination */

        curNo ← srcNo  /* Initialize source node */
        closeNods ← {}  /* Closed list to track visited nodes */
        closeNods.Add(curNo)  /* Add source node to the Closed List
                                 */

        while true do
            /* Retrieve neighbor nodes and their distances from the 
               current node */
        (srcNeighbors, srcDistances, nbrMx) ← GetNeighborInfo(curNo)

            /* Initialize variables for path selection */
            ghCostMn ← MaxValue  /* Placeholder for minimum heuristic
						cost */
            iMn ← -1  /* Index of the next node in the optimal path 
				*/
            fCostMin ← MaxValue  /* Placeholder for minimum f(n) 
						value */

            /* Iterate through all neighboring nodes */
            for i ← 0 to nbrMx - 1 do
                ng1 ← srcNeighbors[i]  /* Select the first neighbor 
							node */

                /* Validate the neighbor:  
                   - Exclude nodes already in the Closed List  
                   - Exclude nodes with depleted battery  
                   - Exclude destination node (handled separately) */
                ng ← GetNg(ng1, closeNods)
                if ng ≥ 0 then
                    hCost ← node2dst[ng]  /* Heuristic cost: 
							   estimated distance to 
							   destination */
                    gCost ← srcDistances[i]  /* Actual cost from 
							   source to neighbor */

                    /* Compute f(n) cost:  
                       - A-Star: f(n) = g(n) + h(n)  
                       - Best-First Search: f(n) = h(n) (ignores 
				g(n)) */
                    fCost ← gCost + hCost  /* A-Star Algorithm */
                    fCost ← hCost  /* Best-First Search Algorithm */

                    /* Select the neighbor with the lowest f(n) */
                    if fCostMin > fCost then
                        fCostMin ← fCost
                        iMn ← ng  /* Store the optimal next node */
                    end if
                end if
            end for

            /* If no valid next node is found, terminate the search 
			*/
            if iMn < 0 then
                break
            end if

            /* Add selected node to the Closed List and update the 
		   current node */
            closeNods.Add(iMn)
            curNo ← iMn

            /* If the destination node is reached, terminate the loop 
			*/
            if curNo = dstNo then
                break
            end if
        end while

        /* Swap source and destination for bidirectional routing */
        m ← srcNo
        srcNo ← dstNo
        dstNo ← m
    end for
**end Algorithm**

**User Guide: Software Usage and Implementation of A-Star and BFS Algorithm in WSN**

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



 
