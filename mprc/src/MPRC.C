#include <stdio.h>
#include <stdlib.h>
#include "mpr.h"
#include "mpr_ext.h"
void usage1(void);
void usage(void);

char *  pickName=0;


char * inFl  =0;
char * outFl  =0;
EXIT * exitF = exit;

int main(int argc, char **argv){
FILE   * pp;
int dflag = 0, i, j;
int gflag = 0;
int gridFlag = 0;
int hflag = 0;
int d_fgr = 50;   /* ñ ýòîé ñêîpîñòè ñ÷èòàòü çàäåpæêó*/
int length = 0;
int  maxSpeed = 0;
double d_delay = 0.0;  /* âpåìÿ çàäåpæêè */
static int             fgrs  [SPEEDS ];
static struct report   left  [SPEEDS ],
                        rigth [SPEEDS ];
if(argc > 1){
/*        for(i = 1; i<argc; i++)  {
             printf("\n%s", argv[i]);
         }
*/        for(  i=1; i<argc; i++){
		if(*argv[i]=='-')
			for(j=1; argv[i][j]!=NULL; j++){
        			 if(argv[i][j]=='G'||argv[i][j]=='g'){
        				gflag = 1;
        			 }
        			 else if(argv[i][j]=='H'||argv[i][j]=='h'){
        				hflag = 1;
        			 }
        			 else if(argv[i][j]=='F'||argv[i][j]=='f'){
                                        d_fgr = atoi(argv[++i]) * 5;
                                        if( d_fgr < START_FGRS)
                                                d_fgr = START_FGRS;
        				break;
        			 }
        			 else if(argv[i][j]=='e'||argv[i][j]=='E'){
        				d_delay = atof(argv[++i]);
        				if(d_delay <0.)
        					d_delay = 0.0;
                                        if(d_delay > 0.5)
                                                d_delay = 0.5;
        				break;
        			 }
        			 else if(argv[i][j]=='I'||argv[i][j]=='i'){
        				inFl = argv[++i];
                                        freopen(inFl,"r",stdin);
        				break;
        			 }
        			 else if(argv[i][j]=='O'||argv[i][j]=='o'){
        				outFl = argv[++i];
                                        freopen(outFl,"w",stdout);
        				break;
        			 }
        			 else if(argv[i][j]=='D'||argv[i][j]=='d'){
        				dflag = atoi(argv[++i]);
        				break;
        			 }
        			 else if(argv[i][j]=='P'||argv[i][j]=='p'){
        				pickName = argv[++i];
        				break;
        			 }
        			 else if(argv[i][j]=='R'||argv[i][j]=='r'){
        			        gridFlag = 1;
        				break;
        			 }
        			 else if(argv[i][j]=='?')   {
        			//	usage();
        				usage1();
        			}	
		        }
		else
			usage();
	}
}
/*
freopen("mpr.out","r",stdin);
freopen(".txt","w",stdout);
*/
for(i=0; i< L_KEEP_ARRAY; i++)	{
      f[i]=1;
      a[i]=1;
}

if(dflag){
	printf("\n* main. figure %d, delay %5.3f cek., global %d, dflag %d",
		d_fgr, d_delay, gflag, dflag);
}
length = report(&maxSpeed,
         fgrs, SPEEDS, left, rigth, d_fgr, d_delay, dflag);



if(hflag)
	out_result( fgrs, left, rigth, length, dflag, gridFlag);
if(gflag)
	out_global( fgrs, left, rigth, length, dflag, gridFlag);

if(pickName){
        int   crit, lastSum, lastHalf, staSp, finSp,
              speed_speed = SPEED_SPEED / 5,
              maxF        = FINISH_FGRS / 5;

	pp = fopen(pickName, "w");
	if(!pp) {
	   perror(pickName);
	   exit(2);
	}

        if(dflag)
                  printf("\n* before lastSpeed (length=%d)", length);

         staSp =  lastPertinentSpeed(&crit, &lastSum, &lastHalf,
                   fgrs, left, rigth, length);
         if(dflag)
                 printf("\n* after  lastSpeed (length=%d)", length);

        if (staSp < START_FGRS/5)      staSp = START_FGRS/5;
        if(maxSpeed < FINISH_FGRS) { //ïðè áîëüøîì SPEEDS ïðîáëåìû ñ ïàìÿòüþ êàêèåòî ???
//            19 + 8 + 8 = 35 SPEEDS

//        if(maxSpeed/5 <  15) {       // íàäî  ïîìåñòèòñÿ â SPEEDS
//               14 + 8 + 8 30 SPEEDS
          finSp = min(staSp + 7 * speed_speed, maxF);
        }
        else {             //áûë âûõîä ïî esc
          finSp = 0;
        }
        fprintf(pp, "%d",  staSp);                    //  çàäàë ñòàðò
        fprintf(pp, "\n%d",finSp);                    //  ôèíèø

        fprintf(pp, "\n4\n%d",staSp);                 //  âûíîñëèâîñòü íà
                                                      //  ñòàðòîâîé
        finSp = min(staSp + 2 * speed_speed, maxF);   //
        fprintf(pp, "\n4\n%d",finSp);                 //  âûíîñëèâîñòü
                                                      // ìàëåíüêèõ îøèáêàõ
        fclose(pp);
}

return(0);

}



void usage(){
	printf("\nresult of  test of human mobility");
	printf("\ncommand line:");
        printf("\nmprá [-gh?] [-r] [-e yy] [-f zz] [-d xx] [-p pick] [-o outfile] -i infile ");
	printf("\n 	where");
        printf("\ng      : total result");
        printf("\nh      : list of results for every speed");
        printf("\nr      : csv - output for gRid");
        printf("\ne yy   : delay /secs/ 0.<= yy <=0.5");
        printf("\nf zz   : to  apply the delay from this speed");
        printf("\nd xx   : debug output of function number xx");
        printf("\np      : to output last pertinent speed to pick file");
	printf("\noutfile: output file");
        printf("\ninfile : input file");
exit(9);
 }
void usage1(){
	printf("\nlist of functions:");
	printf("\n%d-\treport",DD_REPORT);
	printf("\n%d-\t\treport_1",DD_REPORT_1);
	printf("\n%d-\tkeep",DD_KEEP);
	printf("\n%d-\t\t\tmpr_lex",DD_MPR_LEX);
	printf("\n%d-\tout_global",DD_OUT_GLOBAL);
	printf("\n%d-\tout_result",DD_OUT_RESULT);
	usage();
}

/*

void usage(){
	printf("\n ­ «¨§ â¥áâ¨p®¢ ­¨ï ¯®¤¢¨¦­®áâ¨ ­¥p¢­ëå ¯p®æ¥áá®¢");
	printf("\nª®¬ ­¤­ ï áâp®ª :");
        printf("\nmprá [-gh?][-d xx] [-e yy] [-f zz] [-p pick] -i infile [-o outfile]");
	printf("\n 	£¤¥");
        printf("\ng      : ¢ë¢®¤ ®¡é¨å à¥§ã«ìâ â®¢");
        printf("\nh      : ¢ë¢®¤ à¥§ã«ìâ â®¢ ¯® áª®à®áâï¬");
        printf("\ne yy   : ¢p¥¬ï § ¤¥p¦ª¨ /cek/ 0.<= yy <=0.5");
        printf("\nf zz   : áª®à®áâì, á ª®â®p®© áç¨â âì § ¤¥p¦ªã 0 < zz ");
        printf("\nd xx   : ®â« ¤®ç­ë© ¢ë¢®¤ ¯®¤¯à®£à ¬¬ë xx");
        printf("\np      : ¢ë¢¥áâ¨ § áç¨â ­­ãî áª®à®áâì ¢ ä ©« pick");
	printf("\noutfile: ¨¬ï ¢ë¢®¤­®£® ä ©« ");
        printf("\ninfile : ¨¬ï ä ©«  c à¥§ã«ìâ â ¬¨ â¥áâ¨à®¢ ­¨ï");
exit(9);
 }
void usage1(){
	printf("\n ­ «¨§ â¥áâ¨p®¢ ­¨ï ¯®¤¢¨¦­®áâ¨ ­¥p¢­ëå ¯p®æ¥áá®¢");
	printf("\ná¯¨á®ª ¯®¤¯à®£à ¬¬:");
	printf("\n%d-\treport",DD_REPORT);
	printf("\n%d-\t\treport_1",DD_REPORT_1);
	printf("\n%d-\tkeep",DD_KEEP);
	printf("\n%d-\t\t\tmpr_lex",DD_MPR_LEX);
	printf("\n%d-\tout_global",DD_OUT_GLOBAL);
	printf("\n%d-\tout_result",DD_OUT_RESULT);
exit(9);
}

*/
