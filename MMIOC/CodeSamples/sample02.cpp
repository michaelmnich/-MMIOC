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



	int arr3[] = { 62, 21, 25, 4, 22, 11, 8 };
	quicksort(arr3, 0, 6); // wywolanie funkcji sortujacej
	cout << endl << "------------------------------------------------" << endl;
	cout << "QUICK Sorted array:";
	printArray(arr3, 6);
	//for (int i = 0; i < 6; i++) // wypisanie posortowanej tablicy
	//	cout << "tablica[" << i << "] = " << arr3[i] << endl;


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

int partition(int tablica[], int p, int r) // dzielimy tablice na dwie czesci, w pierwszej wszystkie liczby sa mniejsze badz rowne x, w drugiej wieksze lub rowne od x
{
	int x = tablica[p]; // obieramy x
	int i = p, j = r, w; // i, j - indeksy w tabeli
	while (true) // petla nieskonczona - wychodzimy z niej tylko przez return j
	{
		while (tablica[j] > x) // dopoki elementy sa wieksze od x
			j--;
		while (tablica[i] < x) // dopoki elementy sa mniejsze od x
			i++;
		if (i < j) // zamieniamy miejscami gdy i < j
		{
			w = tablica[i];
			tablica[i] = tablica[j];
			tablica[j] = w;
			i++;
			j--;
		}
		else // gdy i >= j zwracamy j jako punkt podzialu tablicy
			return j;
	}
}

void quicksort(int tablica[], int p, int r) // sortowanie szybkie
{
	int q;
	if (p < r)
	{
		q = partition(tablica, p, r); // dzielimy tablice na dwie czesci; q oznacza punkt podzialu
		quicksort(tablica, p, q); // wywolujemy rekurencyjnie quicksort dla pierwszej czesci tablicy
		quicksort(tablica, q + 1, r); // wywolujemy rekurencyjnie quicksort dla drugiej czesci tablicy
	}
}


// This is code is contributed by rathbhupendra 


