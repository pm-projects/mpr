#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include "mpr.h"
#include "mpr_ext.h"
		/*                      ������ ������ ������
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
			� ��������� ������� ����� ������t������ �p��� ����������.
			��� ����� ������ �� press_last.
/*/
  /*
for (i=0; i<length; i++){
printf("\n* ����� ����� %d", fgrs[i]);
printf("\n*     |�������|           ���          |��������� �����|  ��������");
printf("\n*�����| ������|���c��.|��������|�������| ����� |���.��.| ����� |���.��.|");
foo= left[i].prs-(((i==length-1)&&(press_last==1))?1:0);
printf("\n%5d | %5d | %5d |  %5d | %5d | %5.2f | %5.2f | %5.2f | %5.2f | �����",
		left[i].fgr,
		left[i].prs,
	 //		left[i].err,		��� ������� ��������
			rigth[i].err,		// ��� ������ ����� �����
				left[i].act_err,
                                    left[i].diff_err,
			//                                rigth[i].diff_err,

(left[i].prs>0)?  left[i].latent_time_rigth/left[i].prs:0.0,
(left[i].act_err)?left[i].latent_time_act_err/left[i].act_err:0.0,
(foo>0)?          left[i].motor_time_rigth/foo:0.0,
(left[i].act_err)?left[i].motor_time_act_err/left[i].act_err:0.0);

foo=rigth[i].prs-(((i==length-1)&&(press_last==2))?1:0);

printf("\n%5d | %5d | %5d |  %5d | %5d | %5.2f | %5.2f | %5.2f | %5.2f | ������",

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
  char *fmt1 = "\n %5d |  %5d | %5d | %5.2f | %5.2f | %5.2f | %5.2f | %5.2f | %5.2f | �����";   
  char *fmt2 = "\n %5d |  %5d | %5d | %5.2f | %5.2f | %5.2f | %5.2f | %5.2f | %5.2f | ������";
  char *fmt1gr = "; %5d ;  %5d ; %5d ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; �����";   
  char *fmt2gr = "; %5d ;  %5d ; %5d ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; %5.2f ; ������";
  char * f=0;


  if(dflag == DD_OUT_RESULT){
      fprintf(stderr, "\n* RESULT.");
  }
  /*/
  			� ��������� ������� ����� ������t������ �p��� ����������.
  			��� ����� ������ �� press_last.
  /*/

  for (i=0; i<length; i++){
    if(gridFlag) {
      printf("\n %d", fgrs[i]);
      f = fmt1gr;
    }
    else {
      printf("\n* ����� ����� %d", fgrs[i]);
      printf("\n*           ���         |    ��������� �����    |       ��������");
      printf("\n*���c��.|������.|�������| ����� |������.|�������| ����� |������.|�������|");
      f = fmt1;
    }
    foo= left[i].prs-(((i==length-1)&&(press_last==1))?1:0);

    printf(f,
    			rigth[i].err,		// ��� ������ ����� �����
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




