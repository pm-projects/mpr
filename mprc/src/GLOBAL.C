#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include "mpr.h"
#include "mpr_ext.h"

//
//
//
//

static int count_speed[]={0, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4,
							  4, 4, 4, 4, 5};
							/* for 30 to 200 */
static int index_speed[]={15,20,25,30,35,40,45,50,55,60,65,70,75,
							  80,85,90,95,100};

//              !!!!!!! ��� ��������� ������� task  �� ����

static TASK   task[] =
{
//   10.������.95
//   {"30 - ��������� �����������",          15,   0},              //0
     {"30 - ������������",                   15,   FINISH_FGRS },   //1
//   {"������ ������������� - 130",          0,    65 },            //2
     {"������ ������������� - 160",          0,   OLD_FINISH_FGRS },//3
     {"������ ������������� - ������������", 0,    FINISH_FGRS },   //4
//   {"130 - ������������",                  65 ,  FINISH_FGRS },
//   {"110 - ������������",                  55 ,  FINISH_FGRS },
 };

static int    task_size = sizeof(task)/ sizeof(TASK);

//
// ������ ��������� ����������� ��������
//
int lastPertinentSpeed( int           *criterion,
                        int           *perSpeedSum,  //  �������� �� ���������
                        int           *perSpeedHalf, //  �������� �� �����������
                        int           fgrs[],
			struct report left[],
			struct report rigth[],
			int           length){
int perSpeed=0, i, j, err_loc, errLeft, errRight, knsLeft, knsRight ;
*criterion = -1;
*perSpeedSum = 0;
*perSpeedHalf= 0;
for (i=0; i<length; i++){
        err_loc  = 0;
        errLeft  = 0;   knsLeft  = 0;
        errRight = 0;   knsRight = 0;

	err_loc+=left[i].err;
	err_loc+=left[i].act_err;
	err_loc+=left[i].diff_err;

        errLeft+=left[i].err;            knsLeft+=rigth[i].err;
        errLeft+=left[i].act_err;        knsLeft+=left[i].act_err;
        errLeft+=rigth[i].diff_err;      knsLeft+=left[i].diff_err;

	err_loc+=rigth[i].err;
	err_loc+=rigth[i].act_err;
	err_loc+=rigth[i].diff_err;

        errRight+=rigth[i].err;          knsRight+=left[i].err;
        errRight+=rigth[i].act_err;      knsRight+=rigth[i].act_err;
        errRight+=left[i].diff_err;      knsRight+=rigth[i].diff_err;

	for(j=0; j<sizeof(index_speed)/sizeof(int); j++)
		if(fgrs[i] == index_speed[j])
			break;
        if(j==sizeof(index_speed)/sizeof(int)){
	      fprintf(stderr,"\n ERROR! �������� �������� %d, �����  �/� %d",
		fgrs[i], i);
	      exit(2);
	}
//
//   �������� ������������� ����
//    1)  ���-�� ���� ������  <= 5% - 5.5% �� ���������� �����

//    2)  min(���������, ����������) * 2 <= max(����������, ���������)
//                       &&
//        min(���������, ����������) <= 2.5% �� ���������� �����
//
//   �� ����� �������� ������ ���������� ���������
//    !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ����� !!!!!!!!!!!!!!!!!!!!!!!!!!!!
//    3)  min(������, �������) * 2 <= max(�������, ������)
//                       &&
//        min(������, �������) <= 2.5% �� ���������� �����
//
//   �� ����� �������� ������ ���������� ���������
//
	if(err_loc <= count_speed[j])   {
                perSpeed     = fgrs[i];
                *perSpeedSum = fgrs[i];
                *criterion   = SUMMCRIT   ;
	}

    /*       ��� ����� �� ���� */


        else if(min(errLeft, errRight) * 2   <=    max(errLeft, errRight)
			   &&
		   min(errLeft, errRight )      <=    count_speed[j] / 2.){
                perSpeed = fgrs[i];
                *perSpeedHalf = fgrs[i];
                *criterion = HALF_ERRCRIT;
        }
        else if(min(knsLeft, knsRight) * 2   <=    max(knsLeft, knsRight)
			   &&
                   min(knsLeft, knsRight )      <=    count_speed[j] / 2.){
                perSpeed      = fgrs[i];
                *perSpeedHalf = fgrs[i];
                *criterion    = HALF_KNSCRIT;
        }   /**/
}

return  (perSpeed*2)/10;
}


int out_global( int           fgrs[],
		struct report left[],
		struct report rigth[],
		int	      length,
		int	      dflag,
		int           gridFlag){
int  lastSpeed = 0, i= 0, crit, lastSum = 0, lastHalf = 0;
char * critN  = "�� ����";
int  repeat = 0;
dflag = dflag;

lastSpeed    = (lastPertinentSpeed(&crit, &lastSum, &lastHalf,
                                    fgrs, left, rigth, length) * 10)/2;

//          !!!!!!! ��� ��������� ������� task �� ����
//task[0].end = lastSpeed;
task[1].bgn = lastSpeed+1;
task[2].bgn = lastSpeed+1;
//task[4].bgn = lastSpeed+1;
   switch(crit) {
   case SUMMCRIT:      critN = "����������"  ;           break;
   case HALF_ERRCRIT:  critN = "����������� (������)" ; break;
   case HALF_KNSCRIT : critN = "����������� (���)" ;     break;
   default:            critN = "�� ������" ;

   }
   if (gridFlag) {
      printf("\n %s ��������; %d"
               , critN, lastSpeed*2);
      printf("\n��������, ���������  ��������; %d", lastSum*2);
      printf("\n��������, ���������� ��������; %d", lastHalf*2);
   }
   else{
      printf("\n* ��������� ��������.�������� %d �������� �� %s ��������",
               lastSpeed*2, critN);
      printf("\n* �������� ����������  �������� %d", lastSum*2);
      printf("\n* �������� ����������� �������� %d", lastHalf*2);
   }
//
//      ����� ������ ������� �������� ������ � ��� ��
//      ���� ���������  ������ ���� (��� ����� ����������)
     repeat = out_firstTable( lastSpeed, fgrs, left, rigth, length, gridFlag);
     out_title(gridFlag);
     out_allLines ( fgrs, left, rigth,length, gridFlag);
     for(i = 0; i < task_size; i++){
           out_line(fgrs, left, rigth, length-repeat,
                                              task[i].title,
                                                  task[i].bgn,
                                                    task[i].end, gridFlag );
     }
     out_lastLine(fgrs, left, rigth,length, repeat, gridFlag);
     
//                            ������� �������
     out_title1(gridFlag);
     out_allLines1 ( fgrs, left, rigth,length, gridFlag);
     for(i = 0; i < task_size; i++){
           out_line1(fgrs, left, rigth, length-repeat,
                                           task[i].title,
                                                  task[i].bgn,
                                                    task[i].end, gridFlag );
     }
     out_lastLine1(fgrs, left, rigth,length, repeat, gridFlag);
return 0;
}


int percent(int x){
float foo ;
    foo =  M_ARIF(START_FGRS, FINISH_FGRS, 5);
return ( x / foo) * 100;
}

int out_firstTable(  int           perSpeed,
                     int           fgrs[],
		     struct report left[],
		     struct report rigth[],
		     int           length,
		     int           gridFlag){
//
//  ���� ������ ��� �������� ����������
//  �� ��� ��� ���� �������� � fgrs ����������
//
  int    repeat;
  int    maxFgrs = 0, i;
  int    knsL=0, knsR=0, errL=0, errR=0, sum=0, eL, eR;

         //  ������ ����� ���������    ��� ��������� !!!
  if(fgrs[length-1]==fgrs[length-2])
         repeat = 8;                     // ������ ������ ��� 8 ����������
  else
         repeat = 0;
  for (i=0; i<length-repeat; i++){
        //  ������ ����� ���������    ��� ��������� !!!
        //  if( maxFgrs >= fgrs[i])   {  repeat = fgrs[i];    break;     }
        //                 //   ������� �� ���������� - ��������� ����
        //
          maxFgrs = fgrs[i];

          eL    = (left[i].err  + left[i].act_err  + rigth[i].diff_err);
          eR    = (rigth[i].err + rigth[i].act_err + left[i].diff_err) ;

          if(15 <= fgrs[i] && fgrs[i] <= 80) {
              sum  += eL + eR;
          }

          if(fgrs[i] > perSpeed)  {
              knsL += (rigth[i].err + left[i].act_err  + left[i].diff_err );
              knsR += (left[i].err  + rigth[i].act_err + rigth[i].diff_err) ;
              errL += eL;
              errR += eR ;
          }
  }


  //printf("\n %d \n", maxFgrs);
  /*
  printf("\n*\n*       ����� ��� � ������ �� ���� ��������� ��� ����������");
  printf(   "\n* --------------------------------------------------------------");
  printf(   "\n*          |        ������     |       ���         |   �����    ");
  printf(   "\n* ---------+-------------------+-------------------|   ------   ");
  printf(   "\n*          | ������  | �����   | �����   | ������  |   � � �    ");
  printf(   "\n* ---------+---------+---------+---------+---------+------------");
  printf(   "\n*  �����   | %6d  | %6d  | %6d  | %6d  |   %6d   ",
  	   errR, errL,  knsL, knsR, sum  );
  printf(   "\n* ---------+---------+---------+---------+---------+------------ ");
  printf(   "\n*  ������� | %6d  | %6d  | %6d  | %6d  |   %6d   ",
  	   percent(errR),
  		percent(errL) ,
  		      percent(knsL),
  			   percent( knsR),
  				   percent(sum )        );
  */
  if (gridFlag){
    printf("\n��� /30 - 160/ ;   %d", percent(sum ));
    printf(
    "\n�������� ������ (�-�)/1-� ������������� - �������� (%% ���-��)/; %d; %d",
               percent(errR - errL), errR-errL );
    printf(
    "\n�������� ���    (�-�)/1-� ������������� - �������� (%% ���-��)/; %d; %d",
              percent(knsL -  knsR), knsL-knsR   );

  }
  else {
    printf("\n* ��� /30 - 160/    %d %% ", percent(sum ));
    printf(
    "\n* �������� ������ (�-�)/1-� ������������� - ��������/ = %d %%, %d ���-��",
               percent(errR - errL), errR-errL );
    printf(
    "\n* �������� ���    (�-�)/1-� ������������� - ��������/ = %d %%, %d ���-��",
              percent(knsL -  knsR), knsL-knsR   );
    printf("\n*\n*");
  }
  return repeat;
}

void out_title(int gridF ){
  if(gridF){
     printf("\n*** errors table ***");
  }
  else {
     printf("\n*\n*                           ������� ������");
     printf(   "\n* ------------------------------------------------------------------------");
     printf(   "\n*                                     |               �����");
     printf(   "\n*                                     |-----------------+-----------------");
     printf(   "\n*                                     |      ������     |        ���");
     printf(   "\n*                                     |-----------------+-----------------");
     printf(   "\n*                                     | ��  | ��  |��-��| ��  | ��  |��-��");
  }
}

void out_title1(int gridF ){
  if (gridF){
     printf("\n*** delays table ***");
  }
  else {
     printf("\n*\n*                           ������� ������");
     printf(   "\n* ------------------------------------------------------------------------");
     printf(   "\n*                                     |        �������� ������� (�-�) ");
     printf(   "\n*                                     |-----------------+-----------------");
     printf(   "\n*                                     |     ���������   |     ��������");
     printf(   "\n*                                     |-----------------+-----------------");
     printf(   "\n*                                     |�����|�����|����.|�����|�����|����");
  }
}




void out_line(          int           fgrs[],
			struct report left[],
			struct report rigth[],
			int           length,
			char * title, int start, int end, int gridFlag){

  int    maxFgrs = 0, i;
  int    knsL=0, knsR=0, errL=0, errR=0;
  for ( i=0; i<length; i++){
     // if( maxFgrs >= fgrs[i])         break;
  		       //   ������� �� ���������� - ��������� ����
      maxFgrs = fgrs[i];

      if(start <= fgrs[i]  &&  fgrs[i] <= end) {
  	knsL += (left[i].act_err + left[i].diff_err + rigth[i].err );
  	knsR += (left[i].err     + rigth[i].act_err + rigth[i].diff_err) ;

  	errL += (left[i].err     + left[i].act_err  + rigth[i].diff_err) ;
  	errR += (rigth[i].err    + rigth[i].act_err + left[i].diff_err) ;
      }
  }
  if(gridFlag){
    printf("\n%-36s; %5d; %5d; %5d; %5d; %5d; %5d", title,
    	   errR, errL,  errR - errL, knsL, knsR, knsL-knsR );

  }
  else {
    printf(   "\n* ------------------------------------+-----+-----+-----+-----+-----+-----");

    printf("\n* %-36s|%5d|%5d|%5d|%5d|%5d|%5d", title,
    	   errR, errL,  errR - errL, knsL, knsR, knsL-knsR );
  }
}


void out_allLines(      int           fgrs[],
			struct report left[],
			struct report rigth[],
			int           length,
		        int           gridFlag){

  int    i;
  int    knsL=0, knsR=0, errL=0, errR=0;
  for ( i=0; i<length; i++){

  	knsL = (left[i].act_err + left[i].diff_err + rigth[i].err );
  	knsR = (left[i].err     + rigth[i].act_err + rigth[i].diff_err) ;

  	errL = (left[i].err     + left[i].act_err  + rigth[i].diff_err) ;
  	errR = (rigth[i].err    + rigth[i].act_err + left[i].diff_err) ;
  	if (gridFlag){
          	printf("\n%-36i; %5d; %5d; %5d; %5d; %5d; %5d", fgrs[i]*2,
          		   errR, errL,  errR - errL, knsL, knsR, knsL-knsR );

        }
        else {
          	printf("\n* ------------------------------------+-----+-----+-----+-----+-----+-----");

          	printf("\n* %-36i|%5d|%5d|%5d|%5d|%5d|%5d", fgrs[i]*2,
          		   errR, errL,  errR - errL, knsL, knsR, knsL-knsR );
  	}
  }

}

void  out_lastLine(     int           fgrs[],
                        struct report left[],
                        struct report rigth[],
                        int           length,
                        int           repeat,
		        int           gridFlag){

  char title [] = "����� ����������";
  int    maxFgrs = 0, i;
  int    knsL=0, knsR=0, errL=0, errR=0;

  for ( i=length-repeat; i<length; i++){

   //   if( maxFgrs <  fgrs[i])     {  maxFgrs = fgrs[i];     continue; }
                         //   ������� �� ���������� - ������ �������

      knsL += (left[i].act_err + left[i].diff_err + rigth[i].err );
      knsR += (left[i].err     + rigth[i].act_err + rigth[i].diff_err) ;

      errL += (left[i].err     + left[i].act_err  + rigth[i].diff_err) ;
      errR += (rigth[i].err    + rigth[i].act_err + left[i].diff_err) ;
  }
  if(gridFlag){
    printf("\n%-36s; %5d; %5d; %5d; %5d; %5d; %5d", title,
               errR, errL,  errR - errL, knsL, knsR, knsL-knsR );
  }
  else{
    printf(   "\n* ------------------------------------+-----+-----+-----+-----+-----+-----");

    printf("\n* %-36s|%5d|%5d|%5d|%5d|%5d|%5d", title,
               errR, errL,  errR - errL, knsL, knsR, knsL-knsR );
  }
}

void out_line1(          int           fgrs[],
			struct report left[],
			struct report rigth[],
			int           length,
			char * title, int start, int end,
		        int           gridFlag){

  int    maxFgrs = 0, i, j=0;
  double latR =0., latA =0.,  latM =0., motR =0., motA=0., motM=0.;

  for ( i=0; i<length; i++){
     // if( maxFgrs >= fgrs[i])         break;
  		       //   ������� �� ���������� - ��������� ����
      maxFgrs = fgrs[i];

      if(start <= fgrs[i]  &&  fgrs[i] <= end) {
         j ++;
         latR += (AVERT( rigth[i].prs, rigth[i].latent_time_rigth))
                     - (AVERT( left[i].prs, left[i].latent_time_rigth));
         latA += (AVERT(rigth[i].act_err,rigth[i].latent_time_act_err)) -
                         (AVERT(left[i].act_err,left[i].latent_time_act_err));
         latM += (AVERT(rigth[i].diff_err, rigth[i].latent_time_diff_err))-
                   (AVERT(left[i].diff_err, left[i].latent_time_diff_err));
         motR += (AVERT(rigth[i].prs, rigth[i].motor_time_rigth))
                    - (AVERT(left[i].prs, left[i].motor_time_rigth));
         motA += (AVERT(rigth[i].act_err, rigth[i].motor_time_act_err)) -
                     (AVERT(left[i].act_err, left[i].motor_time_act_err));
         motM += (AVERT( rigth[i].diff_err, rigth[i].motor_time_diff_err ))
                     - (AVERT(left[i].diff_err, left[i].motor_time_diff_err));
      }
  }
  if (gridFlag) {
    printf("\n%-36s; %5.2f; %5.2f; %5.2f; %5.2f; %5.2f; %5.2f", title,
                AVERT(j,latR),
                     AVERT(j,latA),
                          AVERT(j,latM),
                                AVERT(j,motR),
                                      AVERT(j,motA),
                                            AVERT(j,motM));
  }
  else {
    printf(   "\n* ------------------------------------+-----+-----+-----+-----+-----+-----");

    printf("\n* %-36s|%5.2f|%5.2f|%5.2f|%5.2f|%5.2f|%5.2f", title,
                AVERT(j,latR),
                     AVERT(j,latA),
                          AVERT(j,latM),
                                AVERT(j,motR),
                                      AVERT(j,motA),
                                            AVERT(j,motM));
  }
}
/*/
			� ��������� ������� ����� ������t������ �p��� ����������.
			��� ����� ������ �� press_last.
/*/


void out_allLines1(      int           fgrs[],
			struct report left[],
			struct report rigth[],
			int           length,
		        int           gridFlag){
  int    i;
  double latR =0., latA =0.,  latM =0., motR =0., motA=0., motM=0.;

  for ( i=0; i<length; i++){
          latR = (AVERT( rigth[i].prs, rigth[i].latent_time_rigth))
                      - (AVERT( left[i].prs, left[i].latent_time_rigth));
          latA = (AVERT(rigth[i].act_err,rigth[i].latent_time_act_err)) -
                       (AVERT(left[i].act_err,left[i].latent_time_act_err));
          latM = (AVERT(rigth[i].diff_err, rigth[i].latent_time_diff_err))-
                    (AVERT(left[i].diff_err, left[i].latent_time_diff_err));
          motR = (AVERT(rigth[i].prs, rigth[i].motor_time_rigth))
                     - (AVERT(left[i].prs, left[i].motor_time_rigth));
          motA = (AVERT(rigth[i].act_err, rigth[i].motor_time_act_err)) -
                     ( AVERT(left[i].act_err, left[i].motor_time_act_err));
          motM = (AVERT( rigth[i].diff_err, rigth[i].motor_time_diff_err ))
                      - (AVERT(left[i].diff_err, left[i].motor_time_diff_err));
          if (gridFlag) {
                printf("\n%-36i; %5.2f; %5.2f; %5.2f; %5.2f; %5.2f; %5.2f", fgrs[i]*2,
                             latR, latA,  latM, motR, motA, motM );
          }
          else {

          	printf(   "\n* ------------------------------------+-----+-----+-----+-----+-----+-----");

                printf("\n* %-36i|%5.2f|%5.2f|%5.2f|%5.2f|%5.2f|%5.2f", fgrs[i]*2,
                             latR, latA,  latM, motR, motA, motM );
          }
  }

}

void  out_lastLine1(    int           fgrs[],
                        struct report left[],
                        struct report rigth[],
                        int           length,
                        int           repeat,
		        int           gridFlag){

  char title [] = "����� ����������";
  int    maxFgrs = 0, i, j=0;
  double latR =0., latA =0.,  latM =0., motR =0., motA=0., motM=0.;

  for ( i=length-repeat; i<length; i++){

                         //   ������� �� ���������� - ������ �������
      j++;
      latR += (AVERT( rigth[i].prs, rigth[i].latent_time_rigth))
                  - (AVERT( left[i].prs, left[i].latent_time_rigth));
      latA += (AVERT(rigth[i].act_err,rigth[i].latent_time_act_err)) -
                      (AVERT(left[i].act_err,left[i].latent_time_act_err));
      latM += (AVERT(rigth[i].diff_err, rigth[i].latent_time_diff_err))-
                (AVERT(left[i].diff_err, left[i].latent_time_diff_err));
      motR += (AVERT(rigth[i].prs, rigth[i].motor_time_rigth))
                 - (AVERT(left[i].prs, left[i].motor_time_rigth));
      motA += (AVERT(rigth[i].act_err, rigth[i].motor_time_act_err)) -
                  (AVERT(left[i].act_err, left[i].motor_time_act_err));
      motM += (AVERT( rigth[i].diff_err, rigth[i].motor_time_diff_err ))
                  - (AVERT(left[i].diff_err, left[i].motor_time_diff_err));

  }
  if (gridFlag){
    printf("\n%-36s; %5.2f; %5.2f; %5.2f; %5.2f; %5.2f; %5.2f", title,
                AVERT(j,latR),
                     AVERT(j,latA),
                          AVERT(j,latM),
                                AVERT(j,motR),
                                      AVERT(j,motA),
                                            AVERT(j,motM));
  }
  else {
    printf(   "\n* ------------------------------------+-----+-----+-----+-----+-----+-----");

    printf("\n* %-36s|%5.2f|%5.2f|%5.2f|%5.2f|%5.2f|%5.2f", title,
                AVERT(j,latR),
                     AVERT(j,latA),
                          AVERT(j,latM),
                                AVERT(j,motR),
                                      AVERT(j,motA),
                                            AVERT(j,motM));
  }
}




