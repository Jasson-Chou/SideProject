#include "ADC_DEMO.h"
extern ADC_HandleTypeDef hadc1;

//One Channel Conversion Start by software.
void Demo_1_1(void)
{
	uint32_t ADC_DR;
	float Volt_mV;
	HAL_ADC_Start(&hadc1);
	HAL_Delay(1);
	ADC_DR = hadc1.Instance->DR;
	Volt_mV = ADC_DR * 0.806f;
	HAL_Delay(100);
}

//EOC with IT
void Demo_1_2()
{
	if(hadc1.State == HAL_ADC_STATE_READY)
	{
			HAL_ADC_Start_IT(&hadc1);
	}
	
	//
	/*
	uint32_t ADC_DR;
float Volt_mV;
void HAL_ADC_ConvCpltCallback(ADC_HandleTypeDef* hadc)
{
  
  UNUSED(hadc);
  
	
	if(hadc->Instance == hadc1.Instance)
	{
		ADC_DR = hadc1.Instance->DR;
		Volt_mV = ADC_DR * 0.806f;
		hadc1.State = HAL_ADC_STATE_READY;
	}
}
	*/
}

void Demo_1_3()
{
//	/* USER CODE BEGIN 2 */
//	HAL_TIM_Base_Start(&htim3);
//	HAL_ADC_Start_DMA(&hadc1, &ADC_DR, 1);
//  /* USER CODE END 2 */

//  /* Infinite loop */
//  /* USER CODE BEGIN WHILE */
//  while (1)
//  {
//    /* USER CODE END WHILE */

//    /* USER CODE BEGIN 3 */
//		HAL_Delay(100);
//		
//		
//  }
//  /* USER CODE END 3 */
}
