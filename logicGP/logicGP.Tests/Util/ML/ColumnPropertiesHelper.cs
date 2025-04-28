namespace logicGP.Tests.Util.ML;

public static class ColumnPropertiesHelper
{
    public static string BalanceScale = "";

    public static string Iris = """
                                [
                                  {
                                    "ColumnName": "sepal length",
                                    "ColumnPurpose": "Feature",
                                    "ColumnDataFormat": "Single",
                                    "IsCategorical": false,
                                    "Type": "Column",
                                    "Version": 5
                                  },
                                  {
                                    "ColumnName": "sepal width",
                                    "ColumnPurpose": "Feature",
                                    "ColumnDataFormat": "Single",
                                    "IsCategorical": false,
                                    "Type": "Column",
                                    "Version": 5
                                  },
                                  {
                                    "ColumnName": "petal length",
                                    "ColumnPurpose": "Feature",
                                    "ColumnDataFormat": "Single",
                                    "IsCategorical": false,
                                    "Type": "Column",
                                    "Version": 5
                                  },
                                  {
                                    "ColumnName": "petal width",
                                    "ColumnPurpose": "Feature",
                                    "ColumnDataFormat": "Single",
                                    "IsCategorical": false,
                                    "Type": "Column",
                                    "Version": 5
                                  },
                                  {
                                    "ColumnName": "class",
                                    "ColumnPurpose": "Label",
                                    "ColumnDataFormat": "String",
                                    "IsCategorical": true,
                                    "Type": "Column",
                                    "Version": 5
                                  }
                                ]
                                """;

    public static string HeartDisease = """
                                        [
                                          {
                                            "ColumnName": "age",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "sex",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "cp",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "trestbps",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "chol",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "fbs",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "restecg",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "thalach",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "exang",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "oldpeak",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": false,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "slope",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "ca",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "thal",
                                            "ColumnPurpose": "Feature",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          },
                                          {
                                            "ColumnName": "num",
                                            "ColumnPurpose": "Label",
                                            "ColumnDataFormat": "Single",
                                            "IsCategorical": true,
                                            "Type": "Column",
                                            "Version": 5
                                          }
                                        ]
                                        """;

    public static string WineQuality { get; set; } = """
        [
          {
            "ColumnName": "fixed_acidity",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "volatile_acidity",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "citric_acid",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "residual_sugar",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "chlorides",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "free_sulfur_dioxide",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "total_sulfur_dioxide",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "density",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "pH",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "sulphates",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "alcohol",
            "ColumnPurpose": "Feature",
            "ColumnDataFormat": "Single",
            "IsCategorical": false,
            "Type": "Column",
            "Version": 5
          },
          {
            "ColumnName": "quality",
            "ColumnPurpose": "Label",
            "ColumnDataFormat": "Single",
            "IsCategorical": true,
            "Type": "Column",
            "Version": 5
          }
        ]
        """;

    public static string BreastCancer { get; set; }
}