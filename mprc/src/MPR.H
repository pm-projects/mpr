#define FGR 	1

#define ANSW    2
#define PRESS	3
#define FREE	4
#define FGRS    5
#define MPR_EOF 0
#define MPR_ERR -1

#define LBUF	300


#define DD_MPR_LEX    1
#define DD_MPR_PARSE  2
#define DD_OUT_RESULT 3
#define DD_OUT_GLOBAL 4
#define DD_REPORT     5
#define DD_REPORT_1   6
#define DD_KEEP       7
#define SAY_ERROR     8
struct report {
	int     fgr;                    // кол-во фигур на текущей скорости
	int     prs;                    // кол-во правильных нажатий
					// на текущей скорости
	int     err;                    /* на фигуру не нажал -  пассивная */
	int     act_err;                /* на 0 нажал фигуру  - активния */
	int     diff_err;	               	/*спутал руки*/
	double  latent_time_rigth;
	double  motor_time_rigth;
	double  latent_time_act_err;
	double  motor_time_act_err;
	double  latent_time_diff_err;
	double  motor_time_diff_err;
	};

typedef struct {               //     тип задание на работу
  char  * title;               //     название строчки
  int     bgn;                 //     считать от сюда
  int     end;                 //     до сюда
}
TASK;

typedef    void EXIT (int status);

extern  EXIT  * exitF;
extern  char * pickName;
#define SUMMCRIT        0
#define HALF_ERRCRIT    1
#define HALF_KNSCRIT    2
		    //               добавил на последней скорости
		    //               возможность работать 12 интервалов
#define SPEEDS  18 + 18
#define SHOW_TIME  	30.
#define START_FGRS	15
#define OLD_FINISH_FGRS 80
#define FINISH_FGRS     100
#define SPEED_SPEED     5
//                             сумма ряда
//                      (первый + последний) * кол_во_членов_ряда      / 2
//
//                      среднее время
#define AVERT(v,t)      (v>0)?t/v:0.0
#define M_ARIF(b,e,s)   ((b)+(e))            * ( ((e)-(b)) / (s) + 1)  /  2
#define L_KEEP_ARRAY    M_ARIF(START_FGRS, FINISH_FGRS, SPEED_SPEED) + 20 * FINISH_FGRS



// to mprc
void initLex(void);
int keep1(int fs[], int lfs, int d_fgr, double d_delay, int dflag);\

int report(     int *maxSpeed,
                int fgrs[],int l_report,
		struct report left[], struct report rigth[],
					 int d_fgr, double d_delay, int dflag);

int report_1(int i, struct report *left,struct report *rigth,int dflag);

int lastPertinentSpeed( int          * criterion,
                        int           *perSpeedSum,  //  скорость по суммарный
                        int           *perSpeedHalf, //  скорость по полушарному
                        int           fgrs[],
			struct report left[],
			struct report rigth[],
			int           length);

int out_global( int           fgrs [],
		struct report left [],
		struct report rigth[],
		int	      length,
		int           dflag,
		int           gridFlag);

int percent(int x);

int  out_firstTable( int           lastPerSpeed,
                     int           fgrs [],
		     struct report left [],
		     struct report rigth[],
		     int           length,
		     int           gridFlag);

void out_title(int gridF);

void  out_line(         int           fgrs [],
			struct report left [],
			struct report rigth[],
			int           length,
			char *        title, 
			int           start, 
			int           end,
		  int           gridFlag);

void  out_allLines(     int           fgrs [],
			struct report left [],
			struct report rigth[],
			int           length,
		        int           gridFlag);

void  out_lastLine(     int           fgrs[],
			struct report left[],
			struct report rigth[],
			int           length,
			int           repeat,
		        int           gridFlag);

int out_result( int           fgrs[],
		struct report left[],
		struct report rigth[],
		int 	      length,
		int           dflag,
		int           gridFlag);

void out_title1(int gridF);

void  out_line1(         int           fgrs [],
			struct report left [],
			struct report rigth[],
			int           length,
			char * title, int start, int end,
		        int           gridFlag);

void  out_allLines1(     int           fgrs [],
			struct report left [],
			struct report rigth[],
			int           length,
		        int           gridFlag);

void  out_lastLine1(     int           fgrs[],
			struct report left[],
			struct report rigth[],
			int           length,
			int           repeat,
		        int           gridFlag);

int mpr_lex(int dflag);














