using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTranslate.Domain.Entities.Response
{
    public class Summary
    {
        public string trackid { get; set; }
        public string source { get; set; }
        public string dis { get; set; }
        public string calorie { get; set; }
        public string end_time { get; set; }
        public string run_time { get; set; }
        public string avg_pace { get; set; }
        public string avg_frequency { get; set; }
        public string avg_heart_rate { get; set; }
        public int type { get; set; }
        public string location { get; set; }
        public string city { get; set; }
        public string forefoot_ratio { get; set; }
        public string bind_device { get; set; }
        public double max_pace { get; set; }
        public double min_pace { get; set; }
        public int version { get; set; }
        public int altitude_ascend { get; set; }
        public int altitude_descend { get; set; }
        public int total_step { get; set; }
        public int avg_stride_length { get; set; }
        public int max_frequency { get; set; }
        public int max_altitude { get; set; }
        public int min_altitude { get; set; }
        public int lap_distance { get; set; }
        public string sync_to { get; set; }
        public int distance_ascend { get; set; }
        public int max_cadence { get; set; }
        public int avg_cadence { get; set; }
        public int landing_time { get; set; }
        public int flight_ratio { get; set; }
        public int climb_dis_descend { get; set; }
        public int climb_dis_ascend_time { get; set; }
        public int climb_dis_descend_time { get; set; }
        public string child_list { get; set; }
        public int parent_trackid { get; set; }
        public int max_heart_rate { get; set; }
        public int min_heart_rate { get; set; }
        public int swolf { get; set; }
        public int total_strokes { get; set; }
        public int total_trips { get; set; }
        public double avg_stroke_speed { get; set; }
        public double max_stroke_speed { get; set; }
        public double avg_distance_per_stroke { get; set; }
        public int swim_pool_length { get; set; }
        public int te { get; set; }
        public int swim_style { get; set; }
        public int unit { get; set; }
        public string add_info { get; set; }
        public int sport_mode { get; set; }
        public int downhill_num { get; set; }
        public int downhill_max_altitude_desend { get; set; }
        public int fore_hand { get; set; }
        public int back_hand { get; set; }
        public int serve { get; set; }
        public int second_half_start_time { get; set; }
        public int rope_skipping_count { get; set; }
        public int rope_skipping_avg_frequency { get; set; }
        public int rope_skipping_max_frequency { get; set; }
        public int rope_skipping_rest_time { get; set; }
        public int left_landing_time { get; set; }
        public int left_flight_ratio { get; set; }
        public int right_landing_time { get; set; }
        public int right_flight_ratio { get; set; }
        public string marathon { get; set; }
        public int situps { get; set; }
        public int anaerobic_te { get; set; }
        public int target_type { get; set; }
        public string target_value { get; set; }
        public int total_group { get; set; }
        public bool auto_recognition { get; set; }
        public string app_name { get; set; }
        public int heartrate_setting_type { get; set; }
    }

    public class Data
    {
        public int next { get; set; }
        public List<Summary> summary { get; set; }
    }

    public class DataSportResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }


}
