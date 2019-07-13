#include <iostream>
#include <stdlib.h>
#include <errno.h>
using namespace std;
int main() 
{
    cout << "Hello, World!";
  
	int a=1;
	
	if(a<0) //some test
	{
		 cout << "A is les than zero"; 
	}
	
	if(a>0/*
	other test
	*/)/*
	other test
	*/{
		 cout << "A is greather than zero"; //test comment 
	}
	system("pause");
	return 0;
	
}

void ffff(){
	 int b=1;
	if(b!=21)
	{
		 cout << "other function if"; //test comment 
	}

}