using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.VVL.Behaviors.Common.CalcIndecator
{
    public class CalcKalmanFilter
    {
        /// <summary>
        /// 1. 상태 변수 정의
        /// </summary>
        // 상태 변수 (State) - 추정하고자 하는 시스템의 상태 (예: 위치, 속도 등)
        // [시간에서의 주식 가격(추정값), 주식가격변화속도(추세)]
        public Matrix x;

        // 상태 공분산 행렬 (State Covariance) - 상태 추정의 불확실성을 나타내는 행렬
        // 
        public Matrix P;


        /// <summary>
        /// 2. 시스템 모델 정의
        /// 시스템 모델은 시간의 흐름에 따라 상태 변수가 어떻게 변하는지를 나타냄
        /// </summary>
        // 상태 전이 행렬 (State Transition Matrix) - 이전 상태에서 현재 상태로의 변화를 나타내는 행렬
        // 가정 : 현재 가격이 이전 가격과 속도에 의해 결정되고 속도는 이전 속도를 유지
        // 가정에 따른 행렬 : [[1,1],[0,1]]
        public Matrix F;

        // 프로세스 노이즈 공분산 행렬 (Process Noise Covariance) - 시스템 모델의 불확실성을 나타내는 행렬
        // 대각 행렬로 주가와 속도의 분산을 나타낸다.
        // [X,0],[V,0] x와 v는 튜닝해야 할 파라미터
        public Matrix Q;

        // 제어 입력 행렬 (Control Matrix) - 제어 입력이 상태에 미치는 영향을 나타내는 행렬 (선택 사항)
        public Matrix B;

        /// <summary>
        /// 3. 측정 모델 정의
        /// 측정 모델은 측정값과 상태 변수간의 관계를 나타낸다.
        /// </summary>

        // 측정 노이즈 공분산 행렬 (Measurement Noise Covariance) - 측정 센서의 노이즈를 나타내는 행렬
        // 주가의 변동성에 따라 설정
        // ex -> R = 1 주가의 노이즈가 큼
        public Matrix R;

        // 관측 행렬 (Observation Matrix) - 상태를 측정값으로 변환하는 행렬
        // 관측 값은 상태 변수와 직접 연결
        public Matrix H;



        // 생성자 (Constructor) - 칼만 필터 초기화
        public CalcKalmanFilter(Matrix x0, Matrix P0, Matrix Q, Matrix R, Matrix F, Matrix H, Matrix B = null)
        {
            this.x = x0.Clone(); // 초기 상태
            this.P = P0.Clone(); // 초기 상태 공분산
            this.Q = Q.Clone(); // 프로세스 노이즈 공분산
            this.R = R.Clone(); // 측정 노이즈 공분산
            this.F = F.Clone(); // 상태 전이 행렬
            this.H = H.Clone(); // 관측 행렬
            this.B = B?.Clone(); // 제어 입력 행렬 (선택 사항)
        }

        // 예측 단계 (Predict) - 다음 상태를 예측
        public void Predict(Matrix u = null) // u: 제어 입력 (선택 사항)
        {
            // 상태 예측: x(k) = F * x(k-1) + B * u(k-1) (제어 입력이 있는 경우)
            if (B != null && u != null)
            {
                this.x = F * this.x + B * u;
            }
            else
            {
                this.x = F * this.x;
            }

            // 공분산 예측: P(k) = F * P(k-1) * F^T + Q
            this.P = F * this.P * F.Transpose() + Q;
        }

        // 업데이트 단계 (Update) - 측정값을 이용하여 상태 추정값 보정
        public void Update(Matrix z) // z: 측정값
        {
            // 예측 오차 공분산 (S = H * P(k-) * H^T + R)
            Matrix S = H * P * H.Transpose() + R;

            // 칼만 이득 (K = P(k-) * H^T * S^-1)
            Matrix K = P * H.Transpose() * S.Inverse();

            // 상태 업데이트: x(k) = x(k-) + K * (z(k) - H * x(k-))
            Matrix y = z - H * x; // 측정 잔차 (Innovation)
            this.x = x + K * y;

            // 공분산 업데이트: P(k) = (I - K * H) * P(k-)
            Matrix I = Matrix.Identity(P.Rows); // 단위 행렬
            this.P = (I - K * H) * P;
        }

        // 상태 벡터를 문자열로 반환 (디버깅 용이)
        public override string ToString()
        {
            return "상태 벡터 (x):\n" + x.ToString() + "\n공분산 행렬 (P):\n" + P.ToString();
        }
    }

    public class Matrix
    {
        private double[,] data;
        public int Rows { get; }
        public int Cols { get; }

        public Matrix(int rows, int cols)
        {
            this.Rows = rows;
            this.Cols = cols;
            this.data = new double[rows, cols];
        }

        public Matrix(double[,] data)
        {
            this.Rows = data.GetLength(0);
            this.Cols = data.GetLength(1);
            this.data = (double[,])data.Clone();
        }

        public double this[int row, int col]
        {
            get { return data[row, col]; }
            set { data[row, col] = value; }
        }

        public Matrix Clone()
        {
            return new Matrix(this.data);
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if(a.Rows != b.Rows || a.Cols != b.Cols)
            {
                throw new ArgumentException("행렬 크기가 일치해야 덧셈이 가능합니다.");
            }
            Matrix RESULT = new Matrix(a.Rows, a.Cols);
            for(int i =0; i< a.Rows; i++)
            {
                for(int j =0; j<a.Cols; j++)
                {
                    RESULT[i, j] = a[i, j] + b[i, j];
                }
            }
            return RESULT;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
            {
                throw new ArgumentException("행렬 크기가 일치해야 뺄셈이 가능합니다.");
            }
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    result[i, j] = a[i, j] - b[i, j];
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Cols != b.Rows)
            {
                throw new ArgumentException("첫 번째 행렬의 열 수와 두 번째 행렬의 행 수가 일치해야 곱셈이 가능합니다.");
            }
            Matrix result = new Matrix(a.Rows, b.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < b.Cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < a.Cols; k++)
                    {
                        sum += a[i, k] * b[k, j];
                    }
                    result[i, j] = sum;
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix a, double scalar)
        {
            Matrix result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Cols; j++)
                {
                    result[i, j] = a[i, j] * scalar;
                }
            }
            return result;
        }

        public Matrix Transpose()
        {
            Matrix result = new Matrix(this.Cols, this.Rows);
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Cols; j++)
                {
                    result[j, i] = this[i, j];
                }
            }
            return result;
        }

        // 역행렬 (Inverse) - 가우스 소거법 사용 (정방 행렬만 가능)
        public Matrix Inverse()
        {
            if (Rows != Cols)
            {
                throw new InvalidOperationException("정방 행렬만 역행렬을 구할 수 있습니다.");
            }

            int n = Rows;
            Matrix augmentedMatrix = new Matrix(n, 2 * n);

            // 확장 행렬 생성 [A | I]
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmentedMatrix[i, j] = this[i, j];
                }
                augmentedMatrix[i, n + i] = 1.0; // 단위 행렬 부분
            }

            // 가우스 소거법 (전진 소거)
            for (int i = 0; i < n; i++)
            {
                // 피벗 선택 (대각 요소가 0이면 아래 행과 교환)
                if (Math.Abs(augmentedMatrix[i, i]) < 1e-9) // 거의 0에 가까우면
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (Math.Abs(augmentedMatrix[j, i]) > 1e-9)
                        {
                            SwapRows(augmentedMatrix, i, j);
                            break;
                        }
                    }
                    if (Math.Abs(augmentedMatrix[i, i]) < 1e-9) // 여전히 0이면 역행렬이 존재하지 않음
                    {
                        throw new InvalidOperationException("역행렬이 존재하지 않습니다 (특이 행렬).");
                    }
                }

                double pivot = augmentedMatrix[i, i];
                for (int j = i; j < 2 * n; j++)
                {
                    augmentedMatrix[i, j] /= pivot; // 정규화
                }

                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        double factor = augmentedMatrix[j, i];
                        for (int k = i; k < 2 * n; k++)
                        {
                            augmentedMatrix[j, k] -= factor * augmentedMatrix[i, k]; // 소거
                        }
                    }
                }
            }

            // 역행렬 부분 추출
            Matrix inverseMatrix = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inverseMatrix[i, j] = augmentedMatrix[i, n + j];
                }
            }
            return inverseMatrix;
        }

        private void SwapRows(Matrix matrix, int row1, int row2)
        {
            for (int j = 0; j < matrix.Cols; j++)
            {
                double temp = matrix[row1, j];
                matrix[row1, j] = matrix[row2, j];
                matrix[row2, j] = temp;
            }
        }

        // 단위 행렬 생성 (Static Method)
        public static Matrix Identity(int size)
        {
            Matrix identity = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                identity[i, i] = 1.0;
            }
            return identity;
        }

        // 행렬 내용을 문자열로 반환 (디버깅 용이)
        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    str += data[i, j].ToString("F4") + "\t"; // 소수점 4자리까지 표시
                }
                str += "\n";
            }
            return str;
        }
    }
}
