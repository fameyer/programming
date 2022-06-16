# Python script for calculating group expenses, where each member spends a different amount of money, but should only
# pay the average.
# Version 2 25.05.2107
import copy
import csv
import numpy as np

# generator to compute permutations of transactions
def generatePermutation(enum = [], permutation = []):
    #print(enum)
    #print(len(enum))
    if len(enum) == 1:
        permutation.append(enum[0])
        #print(permutation)
        yield permutation

    for i in enum:
        perm_copy = copy.deepcopy(permutation)
        perm_copy.append(i)
        enum_copy = copy.deepcopy(enum)
        enum_copy.remove(i)
        for perm in generatePermutation(enum_copy, perm_copy):
            yield perm
            
def generatePermutation1(enum = [], permutation = []):
    #print(enum)
    #print(len(enum))
    if len(enum) == 1:
        permutation.append(enum[0])
        #print(permutation)
        yield permutation

    for i in enum:
        perm_copy = copy.deepcopy(permutation)
        perm_copy.append(i)
        enum_copy = copy.deepcopy(enum)
        enum_copy.remove(i)
        for perm in generatePermutation(enum_copy, perm_copy):
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
	for giver_permutation in self.generatePermutation(ind_give):
		for taker_permutation in self.generatePermutation1(ind_take):   

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

def main():
    # List of CSV entries
    csv_list = []

    # Open given CSV-file
    with open('test.csv', 'r') as csvfile:
        spamreader = csv.reader(csvfile, delimiter=';')

        # save each row as a list element plus a last entry for money to be paid or given
        for row in spamreader:
            csv_list.append(row+[0])

        # get rid of first obsolete entry in csv
        csv_list.pop(0)

        # cast strings of 'Ausgaben' to real float and to 50 Cent
        for i in range(0, len(csv_list)):
            csv_list[i][2] = float(csv_list[i][2])

        # 1st row = Personname, 2nd row = Rolle, 3rd row = expenses
        # Calculate all expenses
        expenses = sum([csv_list[i][2] for i in range(len(csv_list))])

        # fill last row of entries with the difference between average debt and already paid money, round in the end
        average = np.round(expenses/float(len(csv_list)), 2)

        for i in range(len(csv_list)):
            csv_list[i][3] = np.rint([(csv_list[i][2]-average)*100])[0]

        # make copy of original list
        ls = copy.deepcopy(csv_list)

        # find indices of givers and takers
        ind_give = [i for i in range(len(ls)) if ls[i][3] < 0]
        ind_take = [i for i in range(len(ls)) if ls[i][3] > 0]

        # the fewest amount to be allowed to be transitioned
        min_amount = 1.0
            
        # call function
        min_transactions = balanceGroup(ind_give, ind_take, min_amount, ls)

        # Show results and important statistics
        print("Total amount of money spent: \t"+str(expenses)+" EUR")
        print("Average for "+str(len(csv_list))+" people: \t\t"+str(average)+" EUR")
        print("People involved: \t\t\t\t") # print("People involved: \t\t\t\t", end="")
        people_out = ""
        for i in range(len(csv_list)):
            people_out += [csv_list[i][0]+", " if i < len(csv_list)-1 else csv_list[i][0]][0]        
        print(people_out)
        print("\nCalculated transactions with minimal count of "+str(len(min_transactions))+":")
        for i in range(len(min_transactions)):
            print("{0}\t\t gibt {1}\t\t {2:4.2f} EUR".format(csv_list[min_transactions[i][0]][0], csv_list[min_transactions[i][1]][0], min_transactions[i][2]*0.01))     

if __name__ == '__main__':
    main()
