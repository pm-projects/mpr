#include <stdio.h>
#include <stdlib.h>
#include "mpr.h"
#include "mpr_ext.h"
#define   MAX

int report(  int   *maxSp,
             int fgrs[],
             int l_report,
		struct report left[], struct report rigth[],
		int d_fgr, double d_delay, int dflag){
  int	l, i;
  *maxSp = 0;

  if(dflag == DD_REPORT){
  	printf("\n* report. before keep1");
  }


  l = keep1( fgrs, l_report,d_fgr, d_delay, dflag);
  if(dflag == DD_REPORT){
  	printf("\n* report. after keep1");
  }

  for(i = 0; i< l; i++){
          *maxSp = max(*maxSp, fgrs[i]);
  	report_1(fgrs[i],&left[i],&rigth[i],dflag);
  	if(dflag == DD_REPORT)
  		printf("\n* report. Cycle = %d", i);
  }
  return(l);

}

int report_1(int i, struct report *left,struct report *rigth,int dflag){
	     //  кол-во фигур на текущей скорости
	     //     отчет по левой руке
	     // 			отчет по правой руке

int	j;

if(dflag == DD_REPORT_1){
	printf("\n* report_1. global pointer %d.",gl_p);
	printf(" figures number %d",i);
}
(*left).fgr = 0;(*left).prs = 0;(*left).err = 0;(*left).act_err     = 0;
(*left).diff_err = 0;
(*left).latent_time_rigth = 0.;(*left).motor_time_rigth  = 0.;
(*left).latent_time_act_err = 0.;(*left).motor_time_act_err  = 0.;
(*left).latent_time_diff_err = 0.;(*left).motor_time_diff_err  = 0.;

(*rigth).fgr = 0;(*rigth).prs = 0;(*rigth).err = 0;(*rigth).act_err	= 0;
(*rigth).diff_err = 0;
(*rigth).latent_time_rigth = 0.;(*rigth).motor_time_rigth  = 0.;
(*rigth).latent_time_act_err = 0.;(*rigth).motor_time_act_err  = 0.;
(*rigth).latent_time_diff_err = 0.;(*rigth).motor_time_diff_err  = 0.;

//	 двигаемся от последней засчитанной фигуры по кол-ву фигур

for(j = gl_p; j < gl_p+i; j++){
	if( dflag == DD_REPORT_1){
		printf("\n cycles number %d", j);
	}
	if( j > L_KEEP_ARRAY){
		printf("\n*** REPORT_1. ARRAY POINTER %d, ARRAY LENGTH %d",
				j, L_KEEP_ARRAY);
		exitF(2);
	}

/*
			table of errors

                          0       1       2       figures
		  0	  x	  l act   diff
		  1	  l pas   x	  r pas
		  2	  diff	  r act   x
	  answers
*/
	if(f[j] == 1 && a[j] == 0) {
		if(dflag == DD_REPORT_1){
			printf("\n number = %d, f=%d, ans=%d /act,left/",
						 j	,f[j] ,  a[j]);
		}
		(*left).act_err++;
		((*left).motor_time_act_err) +=mt[j];
		((*left).latent_time_act_err)+=lt[j];
	}
	if(f[j] == 1 && a[j] == 2) {

		if(dflag == DD_REPORT_1){
			printf("\n number = %d, f=%d, ans=%d /act,rigth/",
						 j	,f[j] ,  a[j]);
		}
		(*rigth).act_err++;
		((*rigth).motor_time_act_err) +=mt[j];
		((*rigth).latent_time_act_err)+=lt[j];

	}
	if(f[j] == 0 && a[j] == 1){

		if(dflag == DD_REPORT_1){
			printf("\n number = %d, f=%d, ans=%d /pass,left/",
						 j	,f[j] ,  a[j]);
		}
		(*left).err++;
	}
	if(f[j] == 0 && a[j] == 0) {
			((*left).prs)++;
		((*left).motor_time_rigth) +=mt[j];
		((*left).latent_time_rigth)+=lt[j];

	}
		  if(f[j] == 0 && a[j] == 2){			     //
					 (*rigth).diff_err++;
					 ((*rigth).motor_time_diff_err) +=mt[j];
					 ((*rigth).latent_time_diff_err)+=lt[j];

		if(dflag == DD_REPORT_1){
			printf("\n number = %d, f=%d, ans=%d /diff,left/",
						 j	,f[j] ,  a[j]);
		}

/*		(*left).err++;
		(*rigth).act_err++;
*/	}
	if(f[j] == 2 && a[j] == 1){

		if(dflag == DD_REPORT_1){
			printf("\n number = %d, f=%d, ans=%d /pas,rigth/",
						 j	,f[j] ,  a[j]);
		}
		(*rigth).err++;
	}
	if(f[j] == 2 && a[j] == 2) {
			((*rigth).prs)++;
		((*rigth).motor_time_rigth) +=mt[j];
		((*rigth).latent_time_rigth)+=lt[j];

	}
	if(f[j] == 2 && a[j] == 0){

		if(dflag == DD_REPORT_1){
			printf("\n number = %d, f=%d, ans=%d /diff,rigth/ ",
						 j	,f[j] ,  a[j]);
		}
					 (*left).diff_err++;
					 ((*left).motor_time_diff_err) +=mt[j];
					 ((*left).latent_time_diff_err)+=lt[j];

/*		(*left).act_err++;
		(*rigth).err++;
*/	}
	if(f[j] == 0)
		((*left).fgr)++;
	if(f[j] == 2)
		((*rigth).fgr)++;

}
gl_p+=i;
return(gl_p);
}
