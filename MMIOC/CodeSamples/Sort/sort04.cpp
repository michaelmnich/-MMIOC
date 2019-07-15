#include <iostream>
#include <stdlib.h>
#include <errno.h>
#define N 30
using namespace std;


void bubbleSort(int arr[], int n);
void swap(int* xp, int* yp);
void printArray(int arr[], int size);
void selectionSort(int arr[], int n);
void quicksort(int tablica[], int p, int r);
void merge(int pocz, int sr, int kon);
void mergesort(int pocz1, int kon1);

int tab[N] = { 30,29,28,27,26,25,1,2,3,4,5,6,7,24,23,22,21,20,19,18,8,9,10,11,17,16,15,13,14,12 };
int t[N];  // Tablica pomocnicza


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



	mergesort(0, N - 1);
	cout << endl << "------------------------------------------------" << endl;
	cout << "MARGE Sorted array:";
	printArray(tab, N - 1);
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
	int i = p;
	int j = r;
	int w; // i, j - indeksy w tabeli
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

//quicksort----------------------------------------------------------------------------------------------------------

void quicksort(int tablica[], int p, int r) // sortowanie szybkie
{
	int q;
	int p1 = p;
	int r1 = r;
	if (p1 < r1)
	{
		q = partition(tablica, p1, r1); // dzielimy tablice na dwie czesci; q oznacza punkt podzialu
		quicksort(tablica, p1, q); // wywolujemy rekurencyjnie quicksort dla pierwszej czesci tablicy
		quicksort(tablica, q + 1, r1); // wywolujemy rekurencyjnie quicksort dla drugiej czesci tablicy
	}
}




/* Scalanie dwoch posortowanych ciagow
tab[pocz...sr] i tab[sr+1...kon] i
wynik zapisuje w tab[pocz...kon] */
void merge(int pocz, int sr, int kon)
{
	int i;
	int j;
	int q;
	for (i = pocz; i <= kon; i++) t[i] = tab[i];  // Skopiowanie danych do tablicy pomocniczej
	i = pocz; j = sr + 1; q = pocz;                 // Ustawienie wskaźników tablic
	int t1=0;
	int t2=0;
	while (i <= sr && j <= kon) {         // Przenoszenie danych z sortowaniem ze zbiorów pomocniczych do tablicy głównej
		t1 = t[i];
		t2 = t[j];

		if (t1 < t2)
		{
			tab[q++] = t[i++];
		}
		else
		{
			tab[q++] = t[j++];
		}
	}
	while (i <= sr) { tab[q++] = t[i++]; } // Przeniesienie nie skopiowanych danych ze zbioru pierwszego w przypadku, gdy drugi zbiór się skończył
}

/* Procedura sortowania tab[pocz...kon] */
void mergesort(int pocz1, int kon1)
{
	int sr;
	int pocz = pocz1;
	int kon = kon1;
	if (pocz < kon) {
		sr = (pocz + kon) / 2;
		mergesort(pocz, sr);    // Dzielenie lewej części
		mergesort(sr + 1, kon);   // Dzielenie prawej części
		merge(pocz, sr, kon);   // Łączenie części lewej i prawej
	}
}


