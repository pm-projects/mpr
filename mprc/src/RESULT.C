#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include "mpr.h"
#include "mpr_ext.h"
		/*                      старая версия вывода
int out_result( int    fgrs[],
		struct report left[],
		struct report rigth[],
		int 	length,
		int 	dflag){
int i,foo;

if(dflag == DD_OUT_RESULT){
	printf("\n* RESULT.");
	}*/
/*/
			в последнем нажатии может отсутсtвовать вpемя отпускания.
			это можно узнать из press_last.
/*/
  /*
for (i=0; i<length; i++){
printf("\n* всего фигур %d", fgrs[i]);
printf("\n*     |нажатий|           КНС          |латентное время|  моторное");
printf("\n*фигур| верных|пасcив.|активных|спутано| верно |акт.ош.| верно |акт.ош.|");
foo= left[i].prs-(((i==length-1)&&(press_last==1))?1:0);
printf("\n%5d | %5d | %5d |  %5d | %5d | %5.2f | %5.2f | %5.2f | %5.2f | левые",
		left[i].fgr,
		left[i].prs,
	 //		left[i].err,		ТАК ГОВОРИЛ ТРОШИХИН
			rigth[i].err,		// ТАК Сказал Игорь Саныч
				left[i].act_err,
                                    left[i].diff_err,
			//                                rigth[i].diff_err,

(left[i].prs>0)?  left[i].latent_time_rigth/left[i].prs:0.0,
(left[i].act_err)?left[i].latent_time_act_err/left[i].act_err:0.0,
(foo>0)?          left[i].motor_time_rigth/foo:0.0,
(left[i].act_err)?left[i].motor_time_act_err/left[i].act_err:0.0);

foo=rigth[i].prs-(((i==length-1)&&(press_last==2))?1:0);

printf("\n%5d | %5d | %5d |  %5d | %5d | %5.2f | %5.2f | %5.2f | %5.2f | правые",

		rigth[i].fgr,
		rigth[i].prs,
	 //		rigth[i].err,
			left[i].err,
				 rigth[i].act_err,
                                       rigth[i].diff_err,
			//                                 left[i].diff_err,
(rigth[i].prs>0)?    rigth[i].latent_time_rigth/rigth[i].prs:0.0,
(rigth[i].act_err>0)?rigth[i].latent_time_act_err/rigth[i].act_err:0.0,
(foo>0)?             rigth[i].motor_time_rigth/foo:0.0,
(rigth[i].act_err>0)?rigth[i].motor_time_act_err/rigth[i].act_err:0.0)
;
}
return(0);
}
*/
int out_result( int    fgrs[],
		struct report left[],
		struct report rigth[],
		int 	length,
		int 	dflag,
		int           gridFlag){
  int i,foo;
  char *fmt1 = "\n %5d |  %5d | %5d | %5.2f | %5.2f | %5.2f | %5.2f | %5.2f | %5.2f | левые";   
  char *fmt2 = "\n %5d |  %5d | %5d | %5.2f | %5.2f | %5.2f | %5.2f | %5.2f | %5.2f | правые";
  char *fmt1gr = "; %5d ;  %5d ; %5d ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; левые";   
  char *fmt2gr = "; %5d ;  %5d ; %5d ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; правые";
  char * f=0;


  if(dflag == DD_OUT_RESULT){
      fprintf(stderr, "\n* RESULT.");
  }
  /*/
  			в последнем нажатии может отсутсtвовать вpемя отпускания.
  			это можно узнать из press_last.
  /*/

  for (i=0; i<length; i++){
    if(gridFlag) {
      printf("\n %d", fgrs[i]);
      f = fmt1gr;
    }
    else {
      printf("\n* всего фигур %d", fgrs[i]);
      printf("\n*           КНС         |    латентное время    |       моторное");
      printf("\n*пасcив.|активн.|спутано| верно |активн.|спутано| верно |активн.|спутано|");
      f = fmt1;
    }
    foo= left[i].prs-(((i==length-1)&&(press_last==1))?1:0);

    printf(f,
    			rigth[i].err,		// ТАК Сказал Игорь Саныч
    				 left[i].act_err,
    				       left[i].diff_err,

    (left[i].prs>0)?   left[i].latent_time_rigth/left[i].prs:           0.0,
    (left[i].act_err)? left[i].latent_time_act_err/left[i].act_err:     0.0,
    (left[i].diff_err)?left[i].latent_time_diff_err/left[i].diff_err:   0.0,
    (foo>0)?           left[i].motor_time_rigth/foo:                    0.0,
    (left[i].act_err)? left[i].motor_time_act_err/left[i].act_err:      0.0,
    (left[i].diff_err)?left[i].motor_time_diff_err/left[i].diff_err:    0.0
     );

    foo=rigth[i].prs-(((i==length-1)&&(press_last==2))?1:0);

    if(gridFlag)  {
      printf("\n %d", fgrs[i]);
      f = fmt2gr;
    }
    else {
      f = fmt2;
    }
    printf(f,
    			  left[i].err,
                                        rigth[i].act_err,
                                             rigth[i].diff_err,
    (rigth[i].prs>0)?     rigth[i].latent_time_rigth/rigth[i].prs        : 0.0,
    (rigth[i].act_err>0)? rigth[i].latent_time_act_err/rigth[i].act_err  : 0.0,
    (rigth[i].diff_err)?  rigth[i].latent_time_diff_err/rigth[i].diff_err: 0.0,
    (foo>0)?              rigth[i].motor_time_rigth/foo                  : 0.0,
    (rigth[i].act_err>0)? rigth[i].motor_time_act_err/rigth[i].act_err   : 0.0,
    (rigth[i].diff_err)?  rigth[i].motor_time_diff_err/rigth[i].diff_err : 0.0)
    ;
  }
  return(0);
}




