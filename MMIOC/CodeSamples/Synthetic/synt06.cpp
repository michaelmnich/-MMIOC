#include <iostream>
#include <stdlib.h>
#include <errno.h>
using namespace std;


int main(int argc, char* argv[])
{
   // cout << "Hello, World!";
    char* cc = argv[1];
	int a = 1;
	int b=1;
	
	
	if(a<0) //some test
	{
		 cout << "IF a<0"; 
	}
	
	if(a>0){
		 cout << "IF a>0"; //test comment 
	}
	
	if(a<=0){
		 cout << "IF a<=0"; //test comment 
	}
		
	if(a>=0){
		 cout << "IF a>=0"; //test comment 
	}
	
	if(a!=0){
		 cout << "IF a!=0"; //test comment 
	}
	
	
	if(a!=0 && b<=1){
		 cout << "IF a!=0 && b<=1"; //test comment 
	}
	
	
	return 0;
	
}

