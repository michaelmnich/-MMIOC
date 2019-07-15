#include <iostream>
#include <stdlib.h>
#include <errno.h>
using namespace std;

void ffff();

int main(int argc, char* argv[])
{
   // cout << "Hello, World!";
    char* cc = argv[1];
	int a=1;
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
	
	
	ffff();
	//system("pause");
	return 0;
	
}

void ffff(){
	
	 int b=1;
	 bool bb=1;
	 if(b!=21)
	 {
		 cout << "IF b!=21"; //test comment 
		if(bb)
		{
			 cout << "IF bb bool"; //test comment 
		}
	 }

}