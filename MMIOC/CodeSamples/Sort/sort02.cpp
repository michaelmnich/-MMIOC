#include <iostream>
#include <stdlib.h>
#include <errno.h>
using namespace std;


void bubbleSort(int arr[], int n);
void swap(int* xp, int* yp);
void printArray(int arr[], int size);
void selectionSort(int arr[], int n);
void quicksort(int tablica[], int p, int r);

// Driver program to test above functions 
int main()
{
	int arr[] = { 64, 34, 25, 12, 22, 11, 90 };
	int n = sizeof(arr) / sizeof(arr[0]);
	bubbleSort(arr, n);
	cout << "BUBLE Sorted array: ";
	printArray(arr, n);
	cout << endl << "------------------------------------------------" << endl;
	int arr2[] = { 64, 25, 12, 22, 11 };
	int n2 = sizeof(arr2) / sizeof(arr2[0]);
	selectionSort(arr2, n2);
	cout << "SELECTION Sorted array:";
	printArray(arr2, n2);





	return 0;
}


void swap(int* xp, int* yp)
{
	int temp = *xp;
	*xp = *yp;
	*yp = temp;
}

// An optimized version of Bubble Sort 
void bubbleSort(int arr[], int n)
{
	int i;
	int j;
	bool swapped;

	int arr01;
	int arr02;
	for (i = 0; i < n - 1; i++)
	{
		swapped = false;
		for (j = 0; j < n - i - 1; j++)
		{
			arr01 = arr[j];
			arr02 = arr[j + 1];
			if (arr01 > arr02)
			{
				swap(&arr[j], &arr[j + 1]);
				swapped = true;
			}
		}

		// IF no two elements were swapped by inner loop, then break 
		if (swapped == false) {
			break;
		}

	}
}

/* Function to print an array */
void printArray(int arr[], int size)
{
	int i;
	for (i = 0; i < size; i++)
		cout << arr[i] << ", ";

}

//quicksort----------------------------------------------------------------------------------------------------------
void selectionSort(int arr[], int n)
{
	int i;
	int j;
	int min_idx;
	int arr01;
	int arr02;
	// One by one move boundary of unsorted subarray 
	for (i = 0; i < n - 1; i++)
	{
		// Find the minimum element in unsorted array 
		min_idx = i;

		for (j = i + 1; j < n; j++) {
			arr01 = arr[j];
			arr02 = arr[min_idx];
			if (arr01 < arr02) {

				min_idx = j;
			}
		}
		// Swap the found minimum element with the first element 
		swap(&arr[min_idx], &arr[i]);
	}
}




// This is code is contributed by rathbhupendra 


