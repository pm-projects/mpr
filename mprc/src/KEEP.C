#include <stdio.h>
#include <stdlib.h>
#include "mpr.h"
#include "mpr_ext.h"


static void _checkSpeed(int  speed){
if(speed < START_FGRS || FINISH_FGRS < speed
           ||
   (speed % SPEED_SPEED) != 0
  ){
   printf("\n*** KEEP. Wrong speed %d (  line %d, %s)",
               speed, mpr_lineno, lex);
   exitF(2);
}
}



/*
			фигуpа с задеpжкой,
				   задеpжка
*/
keep1(int fs[], int lfs, int d_fgr, double d_delay, int dflag){

int     pfs=0,      pf=-1,      pa,     nlex,   ans,   i , lexI;
/*   ук.колва фигуp,ук.фигуpы, ук.ответа */
double	add,	time,	wait, 	show,	show_old,new_show;
/*			эта каpтина или пpедыдущая*/
if(dflag == DD_KEEP){
	printf("\n* Keep.");
}
initLex();
if(dflag == DD_KEEP){
	printf("\n* Keep. 2");
}
nlex = mpr_lex(dflag);
if(nlex != FGRS){
       printf("\n*** KEEP. Unexpected lex number %d, line %d, %s",
               nlex, mpr_lineno, lex);
       exitF(2);
}
lexI = atoi(lex);
_checkSpeed(lexI);

show = SHOW_TIME / lexI;
show_old = show;		/* длительность показа*/
add  = -show-1.;

do{
	if(nlex != FGRS){
                printf("\n*** KEEP. Unexpected lex number %d, line %d, %s",
                        nlex, mpr_lineno, lex);
                exitF(2);
	}
        lexI = atoi(lex);
        _checkSpeed(lexI);
        fs[pfs] = lexI;
	new_show =  SHOW_TIME / fs[pfs];
	wait = (fs[pfs] < d_fgr)? 0.: d_delay;

	for(i = 1; i <= fs[pfs] ; ){
		if(i ==2 )
			show_old = show;
		nlex = mpr_lex(dflag);
		if( nlex == FGR){
			i++;
			pf++ ;
			f[pf] = (char) atoi(lex);
			add += show;
			show = new_show;
		}
		else if( nlex == ANSW){
			if( press_last!=0 ){
			  printf(
			  "\n* ПРЕДУПРЕЖДЕНИЕ !! Повтоpное нажатие. стpока #%d",
			  mpr_lineno);
			}
			ans = atoi(lex);
			press_last = ans;
			if((nlex=mpr_lex(dflag))!=PRESS){
                                printf("\n*** KEEP. ANSW. Unexpected lex number %d, line %d, %s",
                                        nlex, mpr_lineno, lex);
                                exitF(2);
			}
			time = atof(lex);
			pa = (wait < time)?pf:((pf>0)?pf-1:0);
			a[pa] = (char) ans;
			lt[pa]= time;
			lt[pa] += (wait>=time)?show_old:0.;
			add   =-time;
		}
		else if( nlex == FREE){
			press_last  = 0;
			add = (add <=0.)?0.: add;
			mt[pa]= atof(lex) + add;
		}
		else {
                        printf("\n*** KEEP. DEFAULT. Unexpected lex number %d, line %d, %s",
                                nlex, mpr_lineno, lex);
                        exitF(2);
		}
	}
	pfs++;
	if( pfs >= lfs){
		printf("\n*** KEEP. OVERFLOW FGRS. POINTER %d, LENGTH %d",
			pfs, lfs);
                exitF(2);
	}
	while(  (nlex = mpr_lex(dflag)) != MPR_EOF){
		if( nlex == ANSW){
			ans = atoi(lex);
			press_last=ans;
			if((nlex=mpr_lex(dflag))!=PRESS){
                                printf("\n*** KEEP. ANSW. Unexpected lex number %d, line %d, %s",
                                        nlex, mpr_lineno, lex);
                                exitF(2);
			}
			time = atof(lex);
			pa = (wait < time)?pf:((pf>0)?pf-1:0);
			a[pa] = (char) ans;
			lt[pa]= time;
			lt[pa] += (wait>=time)?show_old:0.;
			add   =-time;
		}
		else if( nlex == FREE){
			press_last=0;
			add = (add <=0.)?0.: add;
			mt[pa]= atof(lex) + add;
		}
		else if (nlex == FGRS)
			break;
		else {
                        printf("\n*** KEEP. DEFAULT. Unexpected lex number %d, line %d, %s",
                                nlex, mpr_lineno, lex);
                        exitF(2);
		}
	}
}
while(nlex != MPR_EOF);

return(pfs);
}
