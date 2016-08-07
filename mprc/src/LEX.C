#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>
#include "mpr.h"

	/*/ it is arrays to keep a test history /*/

char	f [L_KEEP_ARRAY];		/*/  figure /*/
char	a [L_KEEP_ARRAY];               /*/  answer /*/
float	lt[L_KEEP_ARRAY];               /*/  latent time /press time /*/
float	mt[L_KEEP_ARRAY];               /*/  motor  time /free  time /*/
int     gl_p = 0;

static char buf[LBUF];
static char *pbuf;
static char *unbuf;
char 	    *plex;
char	    *lex;
int	press_last=0;
int 	mpr_lineno = 0;


void initLex(){
gl_p = 0;

//*pbuf = 0;
//*unbuf = 0;
//*plex = 0;
//*lex = 0;
*buf = 0;
pbuf = buf;
unbuf = NULL;
plex = buf;
lex = buf;
press_last=0;
mpr_lineno = 0;
}


int mpr_lex(int dflag){
int 	rtrn;
if(dflag == DD_MPR_LEX){
	printf("\n* lex.");
}
if( unbuf == NULL)
    while(*pbuf==NULL||*pbuf=='*'||*pbuf=='+'||isspace(*pbuf)||pbuf==NULL){
	if(*pbuf == '*' || *pbuf == '+'|| *pbuf == NULL||pbuf==NULL){
		pbuf = gets(buf);
		mpr_lineno++;
		if(dflag == DD_MPR_LEX){
			printf("read #%d.",mpr_lineno);
		}
	}
	if(isspace(*pbuf))
		pbuf++;
    }
else   {
	pbuf = unbuf;
	unbuf= NULL;
}
if(dflag == DD_MPR_LEX){
	printf(". #%d.",mpr_lineno);
}
if( pbuf == 0 || *pbuf == '#')
	return(MPR_EOF);

rtrn = MPR_ERR;
lex = pbuf;
if( *lex == 'f' || *lex == 'F') {
	lex = ++pbuf;
	while(isdigit(*pbuf))
		pbuf++;
	if(lex != pbuf)
		rtrn = FGRS;
}
else if( (isdigit(*pbuf) && *(pbuf+1)==',')
	||(isdigit(*pbuf) && isspace(*(pbuf+1)))){
	pbuf+=2;
	rtrn = FGR;
}
else if( (isdigit(*pbuf) && *(pbuf+1)==NULL)){
	pbuf++;
	rtrn = FGR;
}
else if( isdigit(*pbuf) && *(pbuf+1) == '_'){
	pbuf++;
	rtrn = ANSW;
}
else if(*pbuf == '_' && isdigit(*(pbuf+1))){
	lex = ++pbuf;
	rtrn = strspn(pbuf, "0123456789.");
	pbuf+=rtrn;
	pbuf = (*pbuf == ',')?pbuf+1: pbuf;
	rtrn = PRESS;
}
else if( isdigit(*pbuf) && *(pbuf+1) == '.'){
	rtrn = strspn(pbuf, "0123456789.");
	pbuf += rtrn;
	pbuf= (*pbuf == ',')? pbuf+1: pbuf;
	rtrn = FREE;
}
return(rtrn);
}



un_lex(){
   unbuf = plex;
  return 0;
}


