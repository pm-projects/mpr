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

//              !!!!!!! при изменении массива task  см туда

static TASK   task[] =
{
//   10.ноября.95
//   {"30 - последняя засчитанная",          15,   0},              //0
     {"30 - максимальная",                   15,   FINISH_FGRS },   //1
//   {"первая незасчитанная - 130",          0,    65 },            //2
     {"первая незасчитанная - 160",          0,   OLD_FINISH_FGRS },//3
     {"первая незасчитанная - максимальная", 0,    FINISH_FGRS },   //4
//   {"130 - максимальная",                  65 ,  FINISH_FGRS },
//   {"110 - максимальная",                  55 ,  FINISH_FGRS },
 };

static int    task_size = sizeof(task)/ sizeof(TASK);

//
// узнает последнюю засчитанную скорость
//
int lastPertinentSpeed( int           *criterion,
                        int           *perSpeedSum,  //  скорость по суммарный
                        int           *perSpeedHalf, //  скорость по полушарному
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
	      fprintf(stderr,"\n ERROR! неверная скорость %d, номер  п/п %d",
		fgrs[i], i);
	      exit(2);
	}
//
//   скорость засчитывается если
//    1)  кол-во всех ошибок  <= 5% - 5.5% от показанных фигур

//    2)  min(ошибокЛев, ошибокПрав) * 2 <= max(ошибокПрав, ошибокЛев)
//                       &&
//        min(ошибокЛев, ошибокПрав) <= 2.5% от показанных фигур
//
//   мы хотим оставить хорошо работающее полушарие
//    !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! новое !!!!!!!!!!!!!!!!!!!!!!!!!!!!
//    3)  min(кнсЛев, кнсПрав) * 2 <= max(кнсПрав, кнсЛев)
//                       &&
//        min(кнсЛев, кнсПрав) <= 2.5% от показанных фигур
//
//   мы хотим оставить хорошо работающее полушарие
//
	if(err_loc <= count_speed[j])   {
                perSpeed     = fgrs[i];
                *perSpeedSum = fgrs[i];
                *criterion   = SUMMCRIT   ;
	}

    /*       ЭТО ОПЯТЬ НЕ НАДО */


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
char * critN  = "ни один";
int  repeat = 0;
dflag = dflag;

lastSpeed    = (lastPertinentSpeed(&crit, &lastSum, &lastHalf,
                                    fgrs, left, rigth, length) * 10)/2;

//          !!!!!!! при изменении массива task см сюда
//task[0].end = lastSpeed;
task[1].bgn = lastSpeed+1;
task[2].bgn = lastSpeed+1;
//task[4].bgn = lastSpeed+1;
   switch(crit) {
   case SUMMCRIT:      critN = "суммарному"  ;           break;
   case HALF_ERRCRIT:  critN = "полушарному (ошибки)" ; break;
   case HALF_KNSCRIT : critN = "полушарному (КНС)" ;     break;
   default:            critN = "ни одному" ;

   }
   if (gridFlag) {
      printf("\n %s критерию; %d"
               , critN, lastSpeed*2);
      printf("\nскорость, суммарный  критерий; %d", lastSum*2);
      printf("\nскорость, полушарный критерий; %d", lastHalf*2);
   }
   else{
      printf("\n* последняя засчитан.скорость %d отобрана по %s критерию",
               lastSpeed*2, critN);
      printf("\n* скорость суммарного  критерия %d", lastSum*2);
      printf("\n* скорость полушарного критерия %d", lastHalf*2);
   }
//
//      вывод первой таблицы подсчета ошибок и КНС по
//      всем скоростям  первой фазы (без учета повторений)
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
     
//                            средние времена
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
//  надо узнать без подсчета повторений
//  до тех пор пока скорости в fgrs возрастают
//
  int    repeat;
  int    maxFgrs = 0, i;
  int    knsL=0, knsR=0, errL=0, errR=0, sum=0, eL, eR;

         //  теперь может оказаться    два максимума !!!
  if(fgrs[length-1]==fgrs[length-2])
         repeat = 8;                     // просто считаю что 8 повторений
  else
         repeat = 0;
  for (i=0; i<length-repeat; i++){
        //  теперь может оказаться    два максимума !!!
        //  if( maxFgrs >= fgrs[i])   {  repeat = fgrs[i];    break;     }
        //                 //   перешли на повторение - разорвать цикл
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
  printf("\n*\n*       суммы КНС и ошибок по всем скоростям без повторений");
  printf(   "\n* --------------------------------------------------------------");
  printf(   "\n*          |        ошибки     |       КНС         |   сумма    ");
  printf(   "\n* ---------+-------------------+-------------------|   ------   ");
  printf(   "\n*          | правая  | левая   | левая   | правая  |   Р Г М    ");
  printf(   "\n* ---------+---------+---------+---------+---------+------------");
  printf(   "\n*  сумма   | %6d  | %6d  | %6d  | %6d  |   %6d   ",
  	   errR, errL,  knsL, knsR, sum  );
  printf(   "\n* ---------+---------+---------+---------+---------+------------ ");
  printf(   "\n*  процент | %6d  | %6d  | %6d  | %6d  |   %6d   ",
  	   percent(errR),
  		percent(errL) ,
  		      percent(knsL),
  			   percent( knsR),
  				   percent(sum )        );
  */
  if (gridFlag){
    printf("\nРГМ /30 - 160/ ;   %d", percent(sum ));
    printf(
    "\nразность ошибок (П-Л)/1-я незасчитанная - максимум (%% кол-во)/; %d; %d",
               percent(errR - errL), errR-errL );
    printf(
    "\nразность КНС    (Л-П)/1-я незасчитанная - максимум (%% кол-во)/; %d; %d",
              percent(knsL -  knsR), knsL-knsR   );

  }
  else {
    printf("\n* РГМ /30 - 160/    %d %% ", percent(sum ));
    printf(
    "\n* разность ошибок (П-Л)/1-я незасчитанная - максимум/ = %d %%, %d кол-во",
               percent(errR - errL), errR-errL );
    printf(
    "\n* разность КНС    (Л-П)/1-я незасчитанная - максимум/ = %d %%, %d кол-во",
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
     printf("\n*\n*                           таблица ошибок");
     printf(   "\n* ------------------------------------------------------------------------");
     printf(   "\n*                                     |               Сумма");
     printf(   "\n*                                     |-----------------+-----------------");
     printf(   "\n*                                     |      ошибок     |        КНС");
     printf(   "\n*                                     |-----------------+-----------------");
     printf(   "\n*                                     | Пр  | Лв  |Пр-Лв| Лв  | Пр  |Лв-Пр");
  }
}

void out_title1(int gridF ){
  if (gridF){
     printf("\n*** delays table ***");
  }
  else {
     printf("\n*\n*                           таблица времен");
     printf(   "\n* ------------------------------------------------------------------------");
     printf(   "\n*                                     |        Разность средних (П-Л) ");
     printf(   "\n*                                     |-----------------+-----------------");
     printf(   "\n*                                     |     латентное   |     моторное");
     printf(   "\n*                                     |-----------------+-----------------");
     printf(   "\n*                                     |Верно|Актив|Спут.|Верно|Актив|Спут");
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
  		       //   перешли на повторение - разорвать цикл
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

  char title [] = "сумма повторений";
  int    maxFgrs = 0, i;
  int    knsL=0, knsR=0, errL=0, errR=0;

  for ( i=length-repeat; i<length; i++){

   //   if( maxFgrs <  fgrs[i])     {  maxFgrs = fgrs[i];     continue; }
                         //   перешли на повторение - начать подсчет

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
  		       //   перешли на повторение - разорвать цикл
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
			в последнем нажатии может отсутсtвовать вpемя отпускания.
			это можно узнать из press_last.
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

  char title [] = "сумма повторений";
  int    maxFgrs = 0, i, j=0;
  double latR =0., latA =0.,  latM =0., motR =0., motA=0., motM=0.;

  for ( i=length-repeat; i<length; i++){

                         //   перешли на повторение - начать подсчет
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




